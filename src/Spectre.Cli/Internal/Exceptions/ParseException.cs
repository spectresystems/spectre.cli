using System;
using System.Collections.Generic;
using System.Linq;
using Spectre.Cli.Internal.Modelling;
using Spectre.Cli.Internal.Parsing;
using Spectre.Cli.Internal.Rendering;

namespace Spectre.Cli.Internal.Exceptions
{
    internal sealed class ParseException : RuntimeException
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

        public static ParseException ExpectedTokenButFoundNull(CommandTreeToken.Kind expected)
        {
            return new ParseException($"Expected to find any token of type '{expected}' but found null instead.");
        }

        public static ParseException ExpectedTokenButFoundOther(CommandTreeToken.Kind expected, CommandTreeToken.Kind found)
        {
            return new ParseException($"Expected to find token of type '{expected}' but found '{found}' instead.");
        }

        public static ParseException OptionHasNoName(string input, CommandTreeToken token)
        {
            return ParseExceptionFactory.Create(input, token, "Option does not have a name.", "Did you forget the option name?");
        }

        public static ParseException OptionValueWasExpected(string input, CommandTreeToken token)
        {
            return ParseExceptionFactory.Create(input, token, "Expected an option value.", "Did you forget the option value?");
        }

        public static ParseException OptionHasNoValue(IEnumerable<string> args, CommandTreeToken token, CommandOption option)
        {
            return ParseExceptionFactory.Create(args, token, $"Option '{option.GetOptionName()}' is defined but no value has been provided.", "No value provided.");
        }

        public static ParseException UnexpectedOption(IEnumerable<string> args, CommandTreeToken token)
        {
            return ParseExceptionFactory.Create(args, token, $"Unexpected option '{token.Value}'.", "Did you forget the command?");
        }

        public static ParseException CannotAssignValueToFlag(IEnumerable<string> args, CommandTreeToken token)
        {
            return ParseExceptionFactory.Create(args, token, "Flags cannot be assigned a value.", "Can't assign value.");
        }

        public static ParseException InvalidShortOptionName(string input, CommandTreeToken token)
        {
            return ParseExceptionFactory.Create(input, token, "Short option does not have a valid name.", "Not a valid name for a short option.");
        }

        public static ParseException LongOptionNameIsMissing(TextBuffer reader, int position)
        {
            var token = new CommandTreeToken(CommandTreeToken.Kind.LongOption, position, string.Empty, "--");
            return ParseExceptionFactory.Create(reader.Original, token, "Invalid long option name.", "Did you forget the option name?");
        }

        public static ParseException LongOptionNameIsOneCharacter(TextBuffer reader, int position, string name)
        {
            var token = new CommandTreeToken(CommandTreeToken.Kind.LongOption, position, name, $"--{name}");
            var reason = $"Did you mean -{name}?";
            return ParseExceptionFactory.Create(reader.Original, token, "Invalid long option name.", reason);
        }

        public static ParseException LongOptionNameStartWithDigit(TextBuffer reader, int position, string name)
        {
            var token = new CommandTreeToken(CommandTreeToken.Kind.LongOption, position, name, $"--{name}");
            return ParseExceptionFactory.Create(reader.Original, token, "Invalid long option name.", "Option names cannot start with a digit.");
        }

        public static ParseException LongOptionNameContainSymbol(TextBuffer reader, int position, char character)
        {
            var name = character.ToString();
            var token = new CommandTreeToken(CommandTreeToken.Kind.LongOption, position, name, name);
            return ParseExceptionFactory.Create(reader.Original, token, "Invalid long option name.", "Invalid character.");
        }

        public static ParseException UnterminatedQuote(string input, CommandTreeToken token)
        {
            return ParseExceptionFactory.Create(input, token, $"Encountered unterminated quoted string '{token.Value}'.", "Did you forget the closing quotation mark?");
        }

        public static ParseException UnknownCommand(CommandModel model, CommandTree node, IEnumerable<string> args, CommandTreeToken token)
        {
            var availableCommands = node?.Command ?? (ICommandContainer)model;
            var suggestion = CommandSuggestor.Suggest(model, node?.Command, token.Value);

            var text = suggestion != null ? $"Did you mean '{suggestion.Name}'?" : "No such command.";
            return ParseExceptionFactory.Create(args, token, $"Unknown command '{token.Value}'.", text);
        }

        public static ParseException CouldNotMatchArgument(IEnumerable<string> args, CommandTreeToken token)
        {
            return ParseExceptionFactory.Create(args, token, $"Could not match '{token.Value}' with an argument.", "Could not match to argument.");
        }

        public static ParseException UnknownOption(IEnumerable<string> args, CommandTreeToken token)
        {
            return ParseExceptionFactory.Create(args, token, $"Unknown option '{token.Value}'.", "Unknown option.");
        }
    }
}
