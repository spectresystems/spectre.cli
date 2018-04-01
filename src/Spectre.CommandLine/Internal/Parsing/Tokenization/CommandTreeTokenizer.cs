using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Spectre.CommandLine.Internal.Exceptions;

namespace Spectre.CommandLine.Internal.Parsing.Tokenization
{
    internal static class CommandTreeTokenizer
    {
        public static CommandTreeTokenStream Tokenize(IEnumerable<string> args)
        {
            var tokens = new List<CommandTreeToken>();
            var position = 0;
            foreach (var arg in args)
            {
                var start = position;
                var reader = new TextBuffer(arg, position);

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

                position++;
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
            return new CommandTreeToken(CommandTreeToken.Kind.String, position, value, value);
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
                throw ParseException.UnterminatedQuote(builder.ToString());
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
                throw ParseException.UnterminatedOption();
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
            if (char.IsWhiteSpace(reader.Peek()))
            {
                throw ParseException.OptionWithoutName();
            }

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
                    throw ParseException.OptionWithoutValidName();
                }
            }

            return result;
        }

        private static CommandTreeToken ScanLongOption(TextBuffer reader, int position)
        {
            reader.Consume('-');

            if (char.IsWhiteSpace(reader.Peek()))
            {
                throw ParseException.OptionWithoutName();
            }

            var name = ScanString(reader);
            return new CommandTreeToken(CommandTreeToken.Kind.LongOption, position, name.Value, $"--{name.Value}");
        }
    }
}
