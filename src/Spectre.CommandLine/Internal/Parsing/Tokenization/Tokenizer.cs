using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;

namespace Spectre.CommandLine.Internal.Parsing.Tokenization
{
    internal static class Tokenizer
    {
        public static TokenStream Tokenize(IEnumerable<string> args)
        {
            var tokens = new List<Token>();
            foreach (var arg in args)
            {
                var reader = new StringReader(arg);
                while (reader.Peek() != -1)
                {
                    EatWhitespace(reader);

                    var character = (char)reader.Peek();
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
            return new TokenStream(tokens);
        }

        private static void EatWhitespace(TextReader reader)
        {
            int ch;
            while ((ch = reader.Peek()) != -1)
            {
                if (!char.IsWhiteSpace((char)ch))
                {
                    break;
                }
                reader.Read();
            }
        }

        private static Token ScanString(TextReader reader)
        {
            var builder = new StringBuilder();
            while (reader.Peek() != -1)
            {
                builder.Append((char)reader.Read());
            }
            return new Token(Token.Type.String, builder.ToString());
        }

        private static Token ScanQuotedString(TextReader reader)
        {
            Debug.Assert(reader.Peek() == '\"', "Expected '\"' token.");
            reader.Read(); // Consume

            var builder = new StringBuilder();
            while (reader.Peek() != -1)
            {
                var character = (char)reader.Peek();
                if (character == '\"')
                {
                    break;
                }
                builder.Append((char)reader.Read());
            }

            if (reader.Peek() != '\"')
            {
                throw new CommandAppException($"Encountered unterminated quote '{builder}'.");
            }

            reader.Read();
            return new Token(Token.Type.String, builder.ToString());
        }

        private static IEnumerable<Token> ScanOptions(TextReader reader)
        {
            var result = new List<Token>();

            Debug.Assert(reader.Peek() == '-', "Expected '-' token.");
            reader.Read(); // Consume

            switch (reader.Peek())
            {
                case -1:
                    throw new CommandAppException("Encountered unterminated option.");
                case '-':
                    result.Add(ScanLongOption(reader));
                    break;
                default:
                    result.AddRange(ScanShortOptions(reader));
                    break;
            }

            return result;
        }

        private static IEnumerable<Token> ScanShortOptions(TextReader reader)
        {
            if (char.IsWhiteSpace((char)reader.Peek()))
            {
                throw new CommandAppException("Option does not have an identifier.");
            }

            var result = new List<Token>();
            while (true)
            {
                if (reader.Peek() == -1)
                {
                    break;
                }

                var current = (char)reader.Peek();
                if (char.IsWhiteSpace(current))
                {
                    break;
                }

                if (char.IsLetter(current))
                {
                    reader.Read(); // Consume
                    result.Add(new Token(Token.Type.ShortOption, current.ToString(CultureInfo.InvariantCulture)));
                }
                else
                {
                    throw new CommandAppException("Option does not have a valid identifier.");
                }
            }

            return result;
        }

        private static Token ScanLongOption(TextReader reader)
        {
            Debug.Assert(reader.Peek() == '-', "Expected '-' token.");
            reader.Read();

            if (char.IsWhiteSpace((char)reader.Peek()))
            {
                throw new CommandAppException("Option does not have an identifier.");
            }

            var name = ScanString(reader);
            return new Token(Token.Type.LongOption, name.Value);
        }
    }
}
