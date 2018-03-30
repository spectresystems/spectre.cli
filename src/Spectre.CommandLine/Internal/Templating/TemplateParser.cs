namespace Spectre.CommandLine.Internal.Templating
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
                    throw TemplateExceptionHelper.ArgumentCannotContainOptions(template, token);
                }

                if (token.TokenKind == TemplateToken.Kind.OptionalValue ||
                    token.TokenKind == TemplateToken.Kind.RequiredValue)
                {
                    if (!string.IsNullOrWhiteSpace(result.Value))
                    {
                        throw TemplateExceptionHelper.MultipleValuesAreNotSupported(template, token);
                    }
                    if (string.IsNullOrWhiteSpace(token.Value))
                    {
                        throw TemplateExceptionHelper.ValuesMustHaveName(template, token);
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
                        throw TemplateExceptionHelper.OptionsMustHaveName(template, token);
                    }
                    if (char.IsDigit(token.Value[0]))
                    {
                        throw TemplateExceptionHelper.OptionNamesCannotStartWithDigit(template, token);
                    }

                    foreach (var character in token.Value)
                    {
                        if (!char.IsLetterOrDigit(character) && character != '-')
                        {
                            throw TemplateExceptionHelper.InvalidCharacterInOptionName(template, token, character);
                        }
                    }
                }

                if (token.TokenKind == TemplateToken.Kind.LongName)
                {
                    if (!string.IsNullOrWhiteSpace(result.LongName))
                    {
                        throw TemplateExceptionHelper.MultipleLongOptionNamesNotAllowed(template, token);
                    }
                    if (token.Value.Length == 1)
                    {
                        throw TemplateExceptionHelper.LongOptionMustHaveMoreThanOneCharacter(template, token);
                    }
                    result.LongName = token.Value;
                }

                if (token.TokenKind == TemplateToken.Kind.ShortName)
                {
                    if (!string.IsNullOrWhiteSpace(result.ShortName))
                    {
                        throw TemplateExceptionHelper.MultipleShortOptionNamesNotAllowed(template, token);
                    }
                    if (token.Value.Length > 1)
                    {
                        throw TemplateExceptionHelper.ShortOptionMustOnlyBeOneCharacter(template, token);
                    }
                    result.ShortName = token.Value;
                }

                if (token.TokenKind == TemplateToken.Kind.RequiredValue ||
                    token.TokenKind == TemplateToken.Kind.OptionalValue)
                {
                    if (!string.IsNullOrWhiteSpace(result.Value))
                    {
                        throw TemplateExceptionHelper.MultipleOptionValuesAreNotSupported(template, token);
                    }
                    if (token.TokenKind == TemplateToken.Kind.OptionalValue)
                    {
                        throw TemplateExceptionHelper.OptionValueCannotBeOptional(template, token);
                    }

                    foreach (var character in token.Value)
                    {
                        if (!char.IsLetterOrDigit(character))
                        {
                            throw TemplateExceptionHelper.InvalidCharacterInValueName(template, token, character);
                        }
                    }

                    result.Value = token.Value;
                }
            }

            if (string.IsNullOrWhiteSpace(result.LongName) &&
                string.IsNullOrWhiteSpace(result.ShortName))
            {
                throw TemplateExceptionHelper.MissingLongAndShortName(template, null);
            }

            return result;
        }
    }
}
