using System.Globalization;
using Spectre.Cli.Internal;

namespace Spectre.Cli.Exceptions
{
    /// <summary>
    /// Represents errors related to parameter templates.
    /// </summary>
    public sealed class TemplateException : ConfigurationException
    {
        /// <summary>
        /// Gets the template that contains the error.
        /// </summary>
        public string Template { get; }

        internal override bool AlwaysPropagateWhenDebugging => true;

        internal TemplateException(string message, string template, IRenderable pretty)
            : base(message, pretty)
        {
            Template = template;
        }

        internal static TemplateException UnexpectedCharacter(string template, int position, char character)
        {
            return TemplateExceptionFactory.Create(
                template,
                new TemplateToken(TemplateToken.Kind.Unknown, position, $"{character}", $"{character}"),
                $"Encountered unexpected character '{character}'.",
                "Unexpected character.");
        }

        internal static TemplateException UnterminatedValueName(string template, TemplateToken token)
        {
            return TemplateExceptionFactory.Create(template, token,
                $"Encountered unterminated value name '{token.Value}'.",
                "Unterminated value name.");
        }

        internal static TemplateException ArgumentCannotContainOptions(string template, TemplateToken token)
        {
            return TemplateExceptionFactory.Create(template, token,
                "Arguments can not contain options.",
                "Not permitted.");
        }

        internal static TemplateException MultipleValuesAreNotSupported(string template, TemplateToken token)
        {
            return TemplateExceptionFactory.Create(template, token,
                "Multiple values are not supported.",
                "Too many values.");
        }

        internal static TemplateException ValuesMustHaveName(string template, TemplateToken token)
        {
            return TemplateExceptionFactory.Create(template, token,
                "Values without name are not allowed.",
                "Missing value name.");
        }

        internal static TemplateException OptionsMustHaveName(string template, TemplateToken token)
        {
            return TemplateExceptionFactory.Create(template, token,
                "Options without name are not allowed.",
                "Missing option name.");
        }

        internal static TemplateException OptionNamesCannotStartWithDigit(string template, TemplateToken token)
        {
            // Rewrite the token to point to the option name instead of the whole string.
            token = new TemplateToken(
                token.TokenKind,
                token.TokenKind == TemplateToken.Kind.ShortName ? token.Position + 1 : token.Position + 2,
                token.Value, token.Value);

            return TemplateExceptionFactory.Create(template, token,
                "Option names cannot start with a digit.",
                "Invalid option name.");
        }

        internal static TemplateException InvalidCharacterInOptionName(string template, TemplateToken token, char character)
        {
            // Rewrite the token to point to the invalid character instead of the whole value.
            token = new TemplateToken(
                token.TokenKind,
                (token.TokenKind == TemplateToken.Kind.ShortName ? token.Position + 1 : token.Position + 2) + token.Value.IndexOf(character),
                token.Value, character.ToString(CultureInfo.InvariantCulture));

            return TemplateExceptionFactory.Create(template, token,
                $"Encountered invalid character '{character}' in option name.",
                "Invalid character.");
        }

        internal static TemplateException LongOptionMustHaveMoreThanOneCharacter(string template, TemplateToken token)
        {
            // Rewrite the token to point to the option name instead of the whole option.
            token = new TemplateToken(token.TokenKind, token.Position + 2, token.Value, token.Value);

            return TemplateExceptionFactory.Create(template, token,
                "Long option names must consist of more than one character.",
                "Invalid option name.");
        }

        internal static TemplateException MultipleShortOptionNamesNotAllowed(string template, TemplateToken token)
        {
            return TemplateExceptionFactory.Create(template, token,
                "Multiple short option names are not supported.",
                "Too many short options.");
        }

        internal static TemplateException ShortOptionMustOnlyBeOneCharacter(string template, TemplateToken token)
        {
            // Rewrite the token to point to the option name instead of the whole option.
            token = new TemplateToken(token.TokenKind, token.Position + 1, token.Value, token.Value);

            return TemplateExceptionFactory.Create(template, token,
                "Short option names can not be longer than one character.",
                "Invalid option name.");
        }

        internal static TemplateException MultipleOptionValuesAreNotSupported(string template, TemplateToken token)
        {
            return TemplateExceptionFactory.Create(template, token,
                "Multiple option values are not supported.",
                "Too many option values.");
        }

        internal static TemplateException InvalidCharacterInValueName(string template, TemplateToken token, char character)
        {
            // Rewrite the token to point to the invalid character instead of the whole value.
            token = new TemplateToken(
                token.TokenKind,
                token.Position + 1 + token.Value.IndexOf(character),
                token.Value, character.ToString(CultureInfo.InvariantCulture));

            return TemplateExceptionFactory.Create(template, token,
                $"Encountered invalid character '{character}' in value name.",
                "Invalid character.");
        }

        internal static TemplateException MissingLongAndShortName(string template, TemplateToken? token)
        {
            return TemplateExceptionFactory.Create(template, token,
                "No long or short name for option has been specified.",
                "Missing option. Was this meant to be an argument?");
        }

        internal static TemplateException ArgumentsMustHaveValueName(string template)
        {
            return TemplateExceptionFactory.Create(template, null,
                "Arguments must have a value name.",
                "Missing value name.");
        }
    }
}
