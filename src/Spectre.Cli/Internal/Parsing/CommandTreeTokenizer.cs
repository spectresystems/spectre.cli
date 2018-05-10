using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Spectre.Cli.Internal.Exceptions;

namespace Spectre.Cli.Internal.Parsing
{
    internal static class CommandTreeTokenizer
    {
        public enum Mode
        {
            Normal = 0,
            Remaining = 1
        }

        public static CommandTreeTokenStream Tokenize(IEnumerable<string> args)
        {
            var tokens = new List<CommandTreeToken>();
            var position = 0;
            var previousReader = default(TextBuffer);
            var context = new CommandTreeTokenizerContext();

            foreach (var arg in args)
            {
                var lastToken = tokens.LastOrDefault();
                if (lastToken?.TokenKind == CommandTreeToken.Kind.Remaining)
                {
                    context.Mode = Mode.Remaining;
                }

                var start = position;
                var reader = new TextBuffer(previousReader, arg);

                switch (context.Mode)
                {
                    case Mode.Normal:
                        position = ParseToken(context, reader, position, start, tokens);
                        break;
                    case Mode.Remaining:
                        position = ParseRemainingToken(reader, position, start, tokens);
                        break;
                    default:
                        throw new InvalidOperationException("Unspecified tokenization mode.");
                }

                previousReader = reader;
            }
            return new CommandTreeTokenStream(tokens);
        }

        private static int ParseToken(CommandTreeTokenizerContext context, TextBuffer reader, int position, int start, List<CommandTreeToken> tokens)
        {
            while (reader.Peek() != -1)
            {
                EatWhitespace(reader);

                if (reader.ReachedEnd)
                {
                    position += reader.Position - start;
                    break;
                }

                var character = reader.Peek();
                if (!char.IsWhiteSpace(character))
                {
                    if (character == '-')
                    {
                        tokens.AddRange(ScanOptions(context, reader));
                    }
                    else
                    {
                        tokens.Add(ScanString(reader));
                    }
                }
            }

            return position;
        }

        private static int ParseRemainingToken(TextBuffer reader, int position, int start, List<CommandTreeToken> tokens)
        {
            var result = new StringBuilder();
            while (reader.Peek() != -1)
            {
                if (reader.ReachedEnd)
                {
                    position += reader.Position - start;
                    break;
                }

                result.Append(reader.Read());
            }

            var value = result.ToString();
            tokens.Add(new CommandTreeToken(CommandTreeToken.Kind.Remaining, position, value, value));

            return position;
        }

        private static void EatWhitespace(TextBuffer reader)
        {
            while (!reader.ReachedEnd)
            {
                if (!char.IsWhiteSpace(reader.Peek()))
                {
                    break;
                }
                reader.Read();
            }
        }

        private static CommandTreeToken ScanString(TextBuffer reader, char[] stop = null)
        {
            if (reader.TryPeek(out var character))
            {
                // Is this a quoted string?
                if (character == '\"')
                {
                    return ScanQuotedString(reader);
                }
            }

            var position = reader.Position;
            var builder = new StringBuilder();
            while (!reader.ReachedEnd)
            {
                var current = reader.Peek();
                if (stop?.Contains(current) ?? false)
                {
                    break;
                }

                reader.Read(); // Consume
                builder.Append(current);
            }

            var value = builder.ToString();
            return new CommandTreeToken(CommandTreeToken.Kind.String, position, value.Trim(), value);
        }

        private static CommandTreeToken ScanQuotedString(TextBuffer reader)
        {
            var position = reader.Position;

            reader.Consume('\"');

            var builder = new StringBuilder();
            while (!reader.ReachedEnd)
            {
                var character = reader.Peek();
                if (character == '\"')
                {
                    break;
                }
                builder.Append(reader.Read());
            }

            if (reader.Peek() != '\"')
            {
                var unterminatedQuote = builder.ToString();
                var token = new CommandTreeToken(CommandTreeToken.Kind.String, position, unterminatedQuote, $"\"{unterminatedQuote}");
                throw ParseException.UnterminatedQuote(reader.Original, token);
            }

            reader.Read();

            var value = builder.ToString();
            return new CommandTreeToken(CommandTreeToken.Kind.String, position, value, $"\"{value}\"");
        }

        private static IEnumerable<CommandTreeToken> ScanOptions(CommandTreeTokenizerContext context, TextBuffer reader)
        {
            var result = new List<CommandTreeToken>();

            var position = reader.Position;

            reader.Consume('-');
            if (!reader.TryPeek(out var character))
            {
                var token = new CommandTreeToken(CommandTreeToken.Kind.ShortOption, position, "-", "-");
                throw ParseException.OptionHasNoName(reader.Original, token);
            }

            switch (character)
            {
                case '-':
                    var option = ScanLongOption(context, reader, position);
                    if (option != null)
                    {
                        result.Add(option);
                    }
                    break;
                default:
                    result.AddRange(ScanShortOptions(reader, position));
                    break;
            }

            if (reader.TryPeek(out character))
            {
                // Encountered a separator?
                if (character == '=' || character == ':')
                {
                    reader.Read(); // Consume
                    if (!reader.TryPeek(out character))
                    {
                        var token = new CommandTreeToken(CommandTreeToken.Kind.String, reader.Position, "=", "=");
                        throw ParseException.OptionValueWasExpected(reader.Original, token);
                    }

                    result.Add(ScanString(reader));
                }
            }

            return result;
        }

        private static IEnumerable<CommandTreeToken> ScanShortOptions(TextBuffer reader, int position)
        {
            var result = new List<CommandTreeToken>();
            while (true)
            {
                if (reader.ReachedEnd)
                {
                    break;
                }

                var current = reader.Peek();
                if (char.IsWhiteSpace(current))
                {
                    break;
                }

                // Encountered a separator?
                if (current == '=' || current == ':')
                {
                    break;
                }

                if (char.IsLetter(current))
                {
                    reader.Read(); // Consume

                    var value = current.ToString(CultureInfo.InvariantCulture);
                    result.Add(result.Count == 0
                        ? new CommandTreeToken(CommandTreeToken.Kind.ShortOption, position, value, $"-{value}")
                        : new CommandTreeToken(CommandTreeToken.Kind.ShortOption, position + result.Count, value, value));
                }
                else
                {
                    // Create a token representing the short option.
                    var tokenPosition = position + 1 + result.Count;
                    var token = new CommandTreeToken(CommandTreeToken.Kind.ShortOption, tokenPosition, current.ToString(), current.ToString());
                    throw ParseException.InvalidShortOptionName(reader.Original, token);
                }
            }

            return result;
        }

        private static CommandTreeToken ScanLongOption(CommandTreeTokenizerContext context, TextBuffer reader, int position)
        {
            reader.Consume('-');

            if (reader.ReachedEnd)
            {
                context.Mode = Mode.Remaining;
                return null;
            }

            var name = ScanString(reader, new[] { '=', ':' });

            // Perform validation of the name.
            if (name.Value.Length == 0)
            {
                throw ParseException.LongOptionNameIsMissing(reader, position);
            }
            if (name.Value.Length == 1)
            {
                throw ParseException.LongOptionNameIsOneCharacter(reader, position, name.Value);
            }
            if (char.IsDigit(name.Value[0]))
            {
                throw ParseException.LongOptionNameStartWithDigit(reader, position, name.Value);
            }
            for (var index = 0; index < name.Value.Length; index++)
            {
                if (!char.IsLetterOrDigit(name.Value[index]) && name.Value[index] != '-')
                {
                    throw ParseException.LongOptionNameContainSymbol(reader, position + 2 + index, name.Value[index]);
                }
            }

            return new CommandTreeToken(CommandTreeToken.Kind.LongOption, position, name.Value, $"--{name.Value}");
        }
    }
}
