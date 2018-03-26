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
                    throw new CommandAppException("Arguments can not contain options.");
                }
                else if (token.TokenKind == TemplateToken.Kind.OptionalValue ||
                    token.TokenKind == TemplateToken.Kind.RequiredValue)
                {
                    if (!string.IsNullOrWhiteSpace(result.Value))
                    {
                        throw new CommandAppException("Multiple values are not supported.");
                    }
                    if (string.IsNullOrWhiteSpace(token.Value))
                    {
                        throw new CommandAppException("Values without name are not allowed.");
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
                        throw new CommandAppException("Options without name are not allowed.");
                    }
                    if (char.IsDigit(token.Value[0]))
                    {
                        throw new CommandAppException("Option names cannot start with a digit.");
                    }

                    foreach (var character in token.Value)
                    {
                        if (!char.IsLetterOrDigit(character) && character != '-')
                        {
                            throw new CommandAppException($"Encountered invalid character '{character}' in option name.");
                        }
                    }
                }

                if (token.TokenKind == TemplateToken.Kind.LongName)
                {
                    if (!string.IsNullOrWhiteSpace(result.LongName))
                    {
                        throw new CommandAppException("Multiple long option names are not supported.");
                    }
                    if (token.Value.Length == 1)
                    {
                        throw new CommandAppException("Long option names must consist of more than one character.");
                    }
                    result.LongName = token.Value;
                }

                if (token.TokenKind == TemplateToken.Kind.ShortName)
                {
                    if (!string.IsNullOrWhiteSpace(result.ShortName))
                    {
                        throw new CommandAppException("Multiple short option names are not supported.");
                    }
                    if (token.Value.Length > 1)
                    {
                        throw new CommandAppException("Short option names can not be longer than one character.");
                    }
                    result.ShortName = token.Value;
                }

                if (token.TokenKind == TemplateToken.Kind.RequiredValue ||
                    token.TokenKind == TemplateToken.Kind.OptionalValue)
                {
                    if (!string.IsNullOrWhiteSpace(result.Value))
                    {
                        throw new CommandAppException("Multiple option values are not supported.");
                    }
                    if (token.TokenKind == TemplateToken.Kind.OptionalValue)
                    {
                        throw new CommandAppException("Option values cannot be optional.");
                    }

                    foreach (var character in token.Value)
                    {
                        if (!char.IsLetterOrDigit(character))
                        {
                            throw new CommandAppException($"Encountered invalid character '{character}' in value name.");
                        }
                    }

                    result.Value = token.Value;
                }
            }

            if (string.IsNullOrWhiteSpace(result.LongName) &&
                string.IsNullOrWhiteSpace(result.ShortName))
            {
                throw new CommandAppException("No long or short name for option has been specified.");
            }

            return result;
        }
    }
}
