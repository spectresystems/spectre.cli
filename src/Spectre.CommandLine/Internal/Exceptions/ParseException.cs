using System;
using System.Collections.Generic;
using Spectre.CommandLine.Internal.Modelling;
using Spectre.CommandLine.Internal.Parsing.Tokenization;
using Spectre.CommandLine.Internal.Rendering;

namespace Spectre.CommandLine.Internal.Exceptions
{
    internal class ParseException : RuntimeException
    {
        public ParseException(string message, IRenderable pretty = null)
            : base(message, pretty)
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
            return new ParseException($"Expected to find any token of type '{expected}' but found null instead.");
        }

        public static ParseException ExpectedTokenButFoundOther(CommandTreeToken.Kind expected, CommandTreeToken.Kind found)
        {
            return new ParseException($"Expected to find token of type '{expected}' but found '{found}' instead.");
        }

        public static ParseException UnexpectedOption(IReadOnlyList<string> args, CommandTreeToken token)
        {
            return ParseExceptionFactory.Create(args, token, $"Unexpected option '{token.Value}'.", "Did you forget the command?");
        }

        public static ParseException UnknownCommand(IReadOnlyList<string> args, CommandTreeToken token)
        {
            return ParseExceptionFactory.Create(args, token, $"Unknown command '{token.Value}'.", "No such command.");
        }

        public static ParseException CouldNotMatchArgument(IReadOnlyList<string> args, CommandTreeToken token)
        {
            return ParseExceptionFactory.Create(args, token, $"Could not match '{token.Value}' with an argument.", "Could not match to argument.");
        }

        public static ParseException CannotAssignValueToFlag(IReadOnlyList<string> args, CommandTreeToken token)
        {
            return ParseExceptionFactory.Create(args, token, "Flags cannot be assigned a value.", "Can't assign value.");
        }

        public static ParseException NoValueForOption(IReadOnlyList<string> args, CommandTreeToken token, CommandOption option)
        {
            return ParseExceptionFactory.Create(args, token, $"Option '{option.GetOptionName()}' is defined but no value has been provided.", "No value provided.");
        }
    }
}
