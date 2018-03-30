using System;
using Spectre.CommandLine.Internal.Modelling;
using Spectre.CommandLine.Internal.Parsing.Tokenization;

namespace Spectre.CommandLine.Internal.Exceptions
{
    internal class ParseException : RuntimeException
    {
        public ParseException(string message)
            : base(message)
        {
        }

        public ParseException(string message, Exception ex)
            : base(message, ex)
        {
        }

        public static ParseException CouldNotCreateSettings(Type settingsType)
        {
            return new ParseException($"Could not create settings of type '{settingsType.FullName}'.");
        }

        public static ParseException CouldNotCreateCommand(Type commandType)
        {
            return new ParseException($"Could not create command of type '{commandType.FullName}'.");
        }

        public static ParseException UnterminatedQuote(string quote)
        {
            return new ParseException($"Encountered unterminated quote '{quote}'.");
        }

        public static ParseException UnterminatedOption()
        {
            return new ParseException("Encountered unterminated option.");
        }

        public static ParseException OptionWithoutName()
        {
            return new ParseException("Option does not have a name.");
        }

        public static ParseException OptionWithoutValidName()
        {
            return new ParseException("Option does not have a valid name.");
        }

        public static ParseException ExpectedTokenButFoundNull(CommandTreeToken.Kind expected)
        {
            var message = $"Expected to find any token of type '{expected}' but found null instead.";
            return new ParseException(message);
        }

        public static ParseException ExpectedTokenButFoundOther(CommandTreeToken.Kind expected, CommandTreeToken.Kind found)
        {
            var message = $"Expected to find token of type '{expected}' but found '{found}' instead.";
            return new ParseException(message);
        }

        public static ParseException UnexpectedOption(CommandTreeToken token)
        {
            return new ParseException($"Unexpected option '{token.Value}'.");
        }

        public static ParseException UnknownCommand(CommandTreeToken token)
        {
            return new ParseException($"Unknown command '{token.Value}'.");
        }

        public static ParseException UnknownTokenKind(CommandTreeToken.Kind kind)
        {
            return new ParseException($"Encountered unknown token kind ('{kind}').");
        }

        public static ParseException CouldNotMatchArgument(CommandTreeToken token)
        {
            return new ParseException($"Could not match '{token.Value}' with an argument.");
        }

        public static ParseException CannotAssignValueToFlag()
        {
            return new ParseException("Flags cannot be assigned a value.");
        }

        public static ParseException NoValueForOption(CommandOption option)
        {
            return new ParseException($"Option '{option.GetOptionName()}' is defined but no value has been provided.");
        }

        public static ParseException NoValueForArgument(CommandArgument argument)
        {
            return new ParseException($"Argument '{argument.Value}' is defined but no value has been provided.");
        }
    }
}
