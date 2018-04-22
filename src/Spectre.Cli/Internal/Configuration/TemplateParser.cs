using Spectre.Cli.Internal.Exceptions;

namespace Spectre.Cli.Internal.Configuration
{
    internal static class TemplateParser
    {
        public sealed class ArgumentResult
        {
            public string Value { get; set; }
            public bool Required { get; set; }
        }

        public sealed class OptionResult
        {
            public string LongName { get; set; }
            public string ShortName { get; set; }
            public string Value { get; set; }
        }

        public static ArgumentResult ParseArgumentTemplate(string template)
        {
            var result = new ArgumentResult();
            foreach (var token in TemplateTokenizer.Tokenize(template))
            {
                if (token.TokenKind == TemplateToken.Kind.ShortName ||
                    token.TokenKind == TemplateToken.Kind.LongName)
                {
                    throw TemplateException.ArgumentCannotContainOptions(template, token);
                }

                if (token.TokenKind == TemplateToken.Kind.OptionalValue ||
                    token.TokenKind == TemplateToken.Kind.RequiredValue)
                {
                    if (!string.IsNullOrWhiteSpace(result.Value))
                    {
                        throw TemplateException.MultipleValuesAreNotSupported(template, token);
                    }
                    if (string.IsNullOrWhiteSpace(token.Value))
                    {
                        throw TemplateException.ValuesMustHaveName(template, token);
                    }

                    result.Value = token.Value;
                    result.Required = token.TokenKind == TemplateToken.Kind.RequiredValue;
                }
            }
            return result;
        }

        public static OptionResult ParseOptionTemplate(string template)
        {
            var result = new OptionResult();
            foreach (var token in TemplateTokenizer.Tokenize(template))
            {
                if (token.TokenKind == TemplateToken.Kind.LongName || token.TokenKind == TemplateToken.Kind.ShortName)
                {
                    if (string.IsNullOrWhiteSpace(token.Value))
                    {
                        throw TemplateException.OptionsMustHaveName(template, token);
                    }
                    if (char.IsDigit(token.Value[0]))
                    {
                        throw TemplateException.OptionNamesCannotStartWithDigit(template, token);
                    }

                    foreach (var character in token.Value)
                    {
                        if (!char.IsLetterOrDigit(character) && character != '-')
                        {
                            throw TemplateException.InvalidCharacterInOptionName(template, token, character);
                        }
                    }
                }

                if (token.TokenKind == TemplateToken.Kind.LongName)
                {
                    if (!string.IsNullOrWhiteSpace(result.LongName))
                    {
                        throw TemplateException.MultipleLongOptionNamesNotAllowed(template, token);
                    }
                    if (token.Value.Length == 1)
                    {
                        throw TemplateException.LongOptionMustHaveMoreThanOneCharacter(template, token);
                    }
                    result.LongName = token.Value;
                }

                if (token.TokenKind == TemplateToken.Kind.ShortName)
                {
                    if (!string.IsNullOrWhiteSpace(result.ShortName))
                    {
                        throw TemplateException.MultipleShortOptionNamesNotAllowed(template, token);
                    }
                    if (token.Value.Length > 1)
                    {
                        throw TemplateException.ShortOptionMustOnlyBeOneCharacter(template, token);
                    }
                    result.ShortName = token.Value;
                }

                if (token.TokenKind == TemplateToken.Kind.RequiredValue ||
                    token.TokenKind == TemplateToken.Kind.OptionalValue)
                {
                    if (!string.IsNullOrWhiteSpace(result.Value))
                    {
                        throw TemplateException.MultipleOptionValuesAreNotSupported(template, token);
                    }
                    if (token.TokenKind == TemplateToken.Kind.OptionalValue)
                    {
                        throw TemplateException.OptionValueCannotBeOptional(template, token);
                    }

                    foreach (var character in token.Value)
                    {
                        if (!char.IsLetterOrDigit(character))
                        {
                            throw TemplateException.InvalidCharacterInValueName(template, token, character);
                        }
                    }

                    result.Value = token.Value;
                }
            }

            if (string.IsNullOrWhiteSpace(result.LongName) &&
                string.IsNullOrWhiteSpace(result.ShortName))
            {
                throw TemplateException.MissingLongAndShortName(template, null);
            }

            return result;
        }
    }
}
