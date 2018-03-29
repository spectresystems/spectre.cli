using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Spectre.CommandLine.Internal.Parsing.Tokenization
{
    internal static class CommandTreeTokenizer
    {
        public static CommandTreeTokenStream Tokenize(IEnumerable<string> args)
        {
            var tokens = new List<CommandTreeToken>();
            foreach (var arg in args)
            {
                var reader = new TextBuffer(arg);
                while (reader.Peek() != -1)
                {
                    EatWhitespace(reader);

                    if (reader.ReachedEnd)
                    {
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
            var builder = new StringBuilder();
            while (!reader.ReachedEnd)
            {
                builder.Append(reader.Read());
            }
            return new CommandTreeToken(CommandTreeToken.Kind.String, builder.ToString());
        }

        private static CommandTreeToken ScanQuotedString(TextBuffer reader)
        {
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
                throw ExceptionHelper.Tree.Tokenization.UnterminatedQuote(builder.ToString());
            }

            reader.Read();
            return new CommandTreeToken(CommandTreeToken.Kind.String, builder.ToString());
        }

        private static IEnumerable<CommandTreeToken> ScanOptions(TextBuffer reader)
        {
            var result = new List<CommandTreeToken>();

            reader.Consume('-');
            if (!reader.TryPeek(out var character))
            {
                throw ExceptionHelper.Tree.Tokenization.UnterminatedOption();
            }

            switch (character)
            {
                case '-':
                    result.Add(ScanLongOption(reader));
                    break;
                default:
                    result.AddRange(ScanShortOptions(reader));
                    break;
            }

            return result;
        }

        private static IEnumerable<CommandTreeToken> ScanShortOptions(TextBuffer reader)
        {
            if (char.IsWhiteSpace(reader.Peek()))
            {
                throw ExceptionHelper.Tree.Tokenization.OptionWithoutName();
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
                    result.Add(new CommandTreeToken(CommandTreeToken.Kind.ShortOption, current.ToString(CultureInfo.InvariantCulture)));
                }
                else
                {
                    throw ExceptionHelper.Tree.Tokenization.OptionWithoutValidName();
                }
            }

            return result;
        }

        private static CommandTreeToken ScanLongOption(TextBuffer reader)
        {
            reader.Consume('-');

            if (char.IsWhiteSpace(reader.Peek()))
            {
                throw ExceptionHelper.Tree.Tokenization.OptionWithoutName();
            }

            var name = ScanString(reader);
            return new CommandTreeToken(CommandTreeToken.Kind.LongOption, name.Value);
        }
    }
}
