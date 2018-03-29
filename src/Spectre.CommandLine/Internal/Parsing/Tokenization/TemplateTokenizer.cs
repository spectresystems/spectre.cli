using System.Collections.Generic;
using System.Text;

namespace Spectre.CommandLine.Internal.Parsing.Tokenization
{
    internal static class TemplateTokenizer
    {
        public static IReadOnlyList<TemplateToken> Tokenize(string template)
        {
            var tokens = new List<TemplateToken>();
            var buffer = new TextBuffer(template);
            while (!buffer.ReachedEnd)
            {
                EatWhitespace(buffer);

                if (!buffer.TryPeek(out var current))
                {
                    break;
                }

                if (current == '-')
                {
                    tokens.Add(ReadOption(buffer));
                }
                else if (current == '|')
                {
                    buffer.Consume('|');
                }
                else if (current == '<')
                {
                    tokens.Add(ReadValue(buffer, true));
                }
                else if (current == '[')
                {
                    tokens.Add(ReadValue(buffer, false));
                }
                else
                {
                    throw ExceptionHelper.Template.Tokenization.UnexpectedToken(current);
                }
            }
            return tokens;
        }

        private static void EatWhitespace(TextBuffer buffer)
        {
            while (!buffer.ReachedEnd)
            {
                var character = buffer.Peek();
                if (!char.IsWhiteSpace(character))
                {
                    break;
                }
                buffer.Read();
            }
        }

        private static TemplateToken ReadOption(TextBuffer buffer)
        {
            buffer.Consume('-');
            if (buffer.IsNext('-'))
            {
                buffer.Consume('-');
                return new TemplateToken(TemplateToken.Kind.LongName, ReadOptionName(buffer));
            }
            return new TemplateToken(TemplateToken.Kind.ShortName, ReadOptionName(buffer));
        }

        private static string ReadOptionName(TextBuffer buffer)
        {
            var builder = new StringBuilder();
            while (!buffer.ReachedEnd)
            {
                var character = buffer.Peek();
                if (char.IsWhiteSpace(character) || character == '|')
                {
                    break;
                }
                builder.Append(buffer.Read());
            }
            return builder.ToString();
        }

        private static TemplateToken ReadValue(TextBuffer buffer, bool required)
        {
            var start = required ? '<' : '[';
            var end = required ? '>' : ']';

            // Consume start of value character (< or [).
            buffer.Consume(start);

            var builder = new StringBuilder();
            while (!buffer.ReachedEnd)
            {
                var character = buffer.Peek();
                if (character == end)
                {
                    break;
                }

                buffer.Read();
                builder.Append(character);
            }

            if (buffer.ReachedEnd)
            {
                throw ExceptionHelper.Template.Tokenization.UnterminatedValueName(builder.ToString());
            }

            // Consume end of value character (> or ]).
            buffer.Consume(end);

            return new TemplateToken(
                required ? TemplateToken.Kind.RequiredValue : TemplateToken.Kind.OptionalValue,
                builder.ToString());
        }
    }
}