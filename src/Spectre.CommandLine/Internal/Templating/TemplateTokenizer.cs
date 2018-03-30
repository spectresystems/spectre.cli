using System.Collections.Generic;
using System.Text;

namespace Spectre.CommandLine.Internal.Templating
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

                if (!buffer.TryPeek(out var character))
                {
                    break;
                }

                if (character == '-')
                {
                    tokens.Add(ReadOption(buffer));
                }
                else if (character == '|')
                {
                    buffer.Consume('|');
                }
                else if (character == '<')
                {
                    tokens.Add(ReadValue(buffer, true));
                }
                else if (character == '[')
                {
                    tokens.Add(ReadValue(buffer, false));
                }
                else
                {
                    throw TemplateExceptionHelper.UnexpectedToken(buffer.Original, buffer.Position, character);
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
            var position = buffer.Position;

            buffer.Consume('-');
            if (buffer.IsNext('-'))
            {
                buffer.Consume('-');
                var longValue = ReadOptionName(buffer);
                return new TemplateToken(TemplateToken.Kind.LongName, position, longValue, $"--{longValue}");
            }
            var shortValue = ReadOptionName(buffer);
            return new TemplateToken(TemplateToken.Kind.ShortName, position, shortValue, $"-{shortValue}");
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

            var position = buffer.Position;
            var kind = required ? TemplateToken.Kind.RequiredValue : TemplateToken.Kind.OptionalValue;

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
                var name = builder.ToString();
                var token = new TemplateToken(kind, position, name, $"{start}{name}");
                throw TemplateExceptionHelper.UnterminatedValueName(buffer.Original, token);
            }

            // Consume end of value character (> or ]).
            buffer.Consume(end);

            // Get the value.
            var value = builder.ToString();

            return new TemplateToken(
                kind,
                position, value, required ? $"<{value}>" : $"[{value}]");
        }
    }
}