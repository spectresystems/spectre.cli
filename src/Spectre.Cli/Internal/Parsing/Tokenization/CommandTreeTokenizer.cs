using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Spectre.Cli.Internal.Exceptions;

namespace Spectre.Cli.Internal.Parsing.Tokenization
{
    internal static class CommandTreeTokenizer
    {
        public static CommandTreeTokenStream Tokenize(IEnumerable<string> args)
        {
            var tokens = new List<CommandTreeToken>();
            var position = 0;
            var previousReader = default(TextBuffer);
            foreach (var arg in args)
            {
                var start = position;
                var reader = new TextBuffer(previousReader, arg);

                while (reader.Peek() != -1)
                {
                    EatWhitespace(reader);

                    if (reader.ReachedEnd)
                    {
                        position += reader.Position - start;
                        break;
                    }

                    var character = reader.Peek();
                    if (character == '\"')
                    {
                        tokens.Add(ScanQuotedString(reader));
                    }
                    else if (character == '-')
                    {
                        tokens.AddRange(ScanOptions(reader));
                    }
                    else
                    {
                        tokens.Add(ScanString(reader));
                    }
                }

                previousReader = reader;
            }
            return new CommandTreeTokenStream(tokens);
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

        private static CommandTreeToken ScanString(TextBuffer reader)
        {
            var position = reader.Position;
            var builder = new StringBuilder();
            while (!reader.ReachedEnd)
            {
                builder.Append(reader.Read());
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

        private static IEnumerable<CommandTreeToken> ScanOptions(TextBuffer reader)
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
                    result.Add(ScanLongOption(reader, position));
                    break;
                default:
                    result.AddRange(ScanShortOptions(reader, position));
                    break;
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

        private static CommandTreeToken ScanLongOption(TextBuffer reader, int position)
        {
            reader.Consume('-');

            if (reader.ReachedEnd)
            {
                var token = new CommandTreeToken(CommandTreeToken.Kind.ShortOption, position, "--", "--");
                throw ParseException.OptionHasNoName(reader.Original, token);
            }

            var name = ScanString(reader);

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
