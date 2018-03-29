using Spectre.CommandLine.Internal.Parsing.Tokenization;

namespace Spectre.CommandLine.Internal.Parsing
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
                    throw ExceptionHelper.Template.Parsing.ArgumentCannotContainOptions();
                }

                if (token.TokenKind == TemplateToken.Kind.OptionalValue ||
                    token.TokenKind == TemplateToken.Kind.RequiredValue)
                {
                    if (!string.IsNullOrWhiteSpace(result.Value))
                    {
                        throw ExceptionHelper.Template.Parsing.MultipleValuesAreNotSupported();
                    }
                    if (string.IsNullOrWhiteSpace(token.Value))
                    {
                        throw ExceptionHelper.Template.Parsing.ValuesMustHaveName();
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
                        throw ExceptionHelper.Template.Parsing.Options.OptionsMustHaveName();
                    }
                    if (char.IsDigit(token.Value[0]))
                    {
                        throw ExceptionHelper.Template.Parsing.Options.OptionNamesCannotStartWithDigit();
                    }

                    foreach (var character in token.Value)
                    {
                        if (!char.IsLetterOrDigit(character) && character != '-')
                        {
                            throw ExceptionHelper.Template.Parsing.Options.InvalidCharacterInOptionName(character);
                        }
                    }
                }

                if (token.TokenKind == TemplateToken.Kind.LongName)
                {
                    if (!string.IsNullOrWhiteSpace(result.LongName))
                    {
                        throw ExceptionHelper.Template.Parsing.Options.MultipleLongOptionNamesNotAllowed();
                    }
                    if (token.Value.Length == 1)
                    {
                        throw ExceptionHelper.Template.Parsing.Options.LongOptionMustHaveMoreThanOneCharacter();
                    }
                    result.LongName = token.Value;
                }

                if (token.TokenKind == TemplateToken.Kind.ShortName)
                {
                    if (!string.IsNullOrWhiteSpace(result.ShortName))
                    {
                        throw ExceptionHelper.Template.Parsing.Options.MultipleShortOptionNamesNotAllowed();
                    }
                    if (token.Value.Length > 1)
                    {
                        throw ExceptionHelper.Template.Parsing.Options.ShortOptionMustOnlyBeOneCharacter();
                    }
                    result.ShortName = token.Value;
                }

                if (token.TokenKind == TemplateToken.Kind.RequiredValue ||
                    token.TokenKind == TemplateToken.Kind.OptionalValue)
                {
                    if (!string.IsNullOrWhiteSpace(result.Value))
                    {
                        throw ExceptionHelper.Template.Parsing.Options.MultipleOptionValuesAreNotSupported();
                    }
                    if (token.TokenKind == TemplateToken.Kind.OptionalValue)
                    {
                        throw ExceptionHelper.Template.Parsing.Options.OptionValueCannotBeOptional();
                    }

                    foreach (var character in token.Value)
                    {
                        if (!char.IsLetterOrDigit(character))
                        {
                            throw ExceptionHelper.Template.Parsing.Options.InvalidCharacterInValueName(character);
                        }
                    }

                    result.Value = token.Value;
                }
            }

            if (string.IsNullOrWhiteSpace(result.LongName) &&
                string.IsNullOrWhiteSpace(result.ShortName))
            {
                throw ExceptionHelper.Template.Parsing.Options.MissingLongAndShortName();
            }

            return result;
        }
    }
}
