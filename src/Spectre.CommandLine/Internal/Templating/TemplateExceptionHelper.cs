namespace Spectre.CommandLine.Internal.Templating
{
    internal static class TemplateExceptionHelper
    {
        public static TemplateException UnexpectedToken(string template, int position, char token)
        {
            return TemplateException.Factory.Create(template, position, $"{token}",
                $"Encountered unexpected token '{token}'.",
                "Unexpected token.");
        }

        public static TemplateException UnterminatedValueName(string template, TemplateToken token)
        {
            return TemplateException.Factory.Create(template, token,
                $"Encountered unterminated value name '{token.Value}'.",
                "Unterminated value name.");
        }

        public static TemplateException ArgumentCannotContainOptions(string template, TemplateToken token)
        {
            return TemplateException.Factory.Create(template, token,
                "Arguments can not contain options.",
                "Not permitted.");
        }

        public static TemplateException MultipleValuesAreNotSupported(string template, TemplateToken token)
        {
            return TemplateException.Factory.Create(template, token,
                "Multiple values are not supported.",
                "Too many values.");
        }

        public static TemplateException ValuesMustHaveName(string template, TemplateToken token)
        {
            return TemplateException.Factory.Create(template, token,
                "Values without name are not allowed.",
                "Missing value name.");
        }

        public static TemplateException OptionsMustHaveName(string template, TemplateToken token)
        {
            return TemplateException.Factory.Create(template, token,
                "Options without name are not allowed.",
                "Missing option name.");
        }

        public static TemplateException OptionNamesCannotStartWithDigit(string template, TemplateToken token)
        {
            return TemplateException.Factory.Create(template, token,
                "Option names cannot start with a digit.",
                "Invalid option name.");
        }

        public static TemplateException InvalidCharacterInOptionName(string template, TemplateToken token, char character)
        {
            return TemplateException.Factory.Create(template, token,
                $"Encountered invalid character '{character}' in option name.",
                "Invalid character.");
        }

        public static TemplateException MultipleLongOptionNamesNotAllowed(string template, TemplateToken token)
        {
            return TemplateException.Factory.Create(template, token,
                "Multiple long option names are not supported.",
                "Too many long options.");
        }

        public static TemplateException LongOptionMustHaveMoreThanOneCharacter(string template, TemplateToken token)
        {
            return TemplateException.Factory.Create(template, token,
                "Long option names must consist of more than one character.",
                "Invalid option name.");
        }

        public static TemplateException MultipleShortOptionNamesNotAllowed(string template, TemplateToken token)
        {
            return TemplateException.Factory.Create(template, token,
                "Multiple short option names are not supported.",
                "Too many short options.");
        }

        public static TemplateException ShortOptionMustOnlyBeOneCharacter(string template, TemplateToken token)
        {
            return TemplateException.Factory.Create(template, token,
                "Short option names can not be longer than one character.",
                "Invalid option name.");
        }

        public static TemplateException MultipleOptionValuesAreNotSupported(string template, TemplateToken token)
        {
            return TemplateException.Factory.Create(template, token,
                "Multiple option values are not supported.",
                "Too many option values.");
        }

        public static TemplateException OptionValueCannotBeOptional(string template, TemplateToken token)
        {
            return TemplateException.Factory.Create(template, token,
                "Option values cannot be optional.",
                "Must be required.");
        }

        public static TemplateException InvalidCharacterInValueName(string template, TemplateToken token, char character)
        {
            return TemplateException.Factory.Create(template, token,
                $"Encountered invalid character '{character}' in value name.",
                "Invalid character.");
        }

        public static TemplateException MissingLongAndShortName(string template, TemplateToken token)
        {
            return TemplateException.Factory.Create(template, token,
                "No long or short name for option has been specified.",
                "Missing option. Was this meant to be an argument?");
        }
    }
}
