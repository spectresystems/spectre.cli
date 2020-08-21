using System;
using System.Collections.Generic;
using System.Globalization;
using Spectre.Cli.Internal;
using Spectre.Console;
using Spectre.Console.Composition;

namespace Spectre.Cli.Exceptions
{
    /// <summary>
    /// Represents errors that occur during parsing.
    /// </summary>
    public sealed class ParseException : RuntimeException
    {
        internal ParseException(string message, IRenderable? pretty = null)
            : base(message, pretty)
        {
        }

        internal static ParseException CouldNotCreateSettings(Type settingsType)
        {
            return new ParseException($"Could not create settings of type '{settingsType.FullName}'.");
        }

        internal static ParseException CouldNotCreateCommand(Type? commandType)
        {
            if (commandType == null)
            {
                return new ParseException($"Could not create command. Command type is unknown.");
            }

            return new ParseException($"Could not create command of type '{commandType.FullName}'.");
        }

        internal static ParseException ExpectedTokenButFoundNull(CommandTreeToken.Kind expected)
        {
            return new ParseException($"Expected to find any token of type '{expected}' but found null instead.");
        }

        internal static ParseException ExpectedTokenButFoundOther(CommandTreeToken.Kind expected, CommandTreeToken.Kind found)
        {
            return new ParseException($"Expected to find token of type '{expected}' but found '{found}' instead.");
        }

        internal static ParseException OptionHasNoName(string input, CommandTreeToken token)
        {
            return ParseExceptionFactory.Create(input, token, "Option does not have a name.", "Did you forget the option name?");
        }

        internal static ParseException OptionValueWasExpected(string input, CommandTreeToken token)
        {
            return ParseExceptionFactory.Create(input, token, "Expected an option value.", "Did you forget the option value?");
        }

        internal static ParseException OptionHasNoValue(IEnumerable<string> args, CommandTreeToken token, CommandOption option)
        {
            return ParseExceptionFactory.Create(args, token, $"Option '{option.GetOptionName()}' is defined but no value has been provided.", "No value provided.");
        }

        internal static ParseException UnexpectedOption(IEnumerable<string> args, CommandTreeToken token)
        {
            return ParseExceptionFactory.Create(args, token, $"Unexpected option '{token.Value}'.", "Did you forget the command?");
        }

        internal static ParseException CannotAssignValueToFlag(IEnumerable<string> args, CommandTreeToken token)
        {
            return ParseExceptionFactory.Create(args, token, "Flags cannot be assigned a value.", "Can't assign value.");
        }

        internal static ParseException InvalidShortOptionName(string input, CommandTreeToken token)
        {
            return ParseExceptionFactory.Create(input, token, "Short option does not have a valid name.", "Not a valid name for a short option.");
        }

        internal static ParseException LongOptionNameIsMissing(TextBuffer reader, int position)
        {
            var token = new CommandTreeToken(CommandTreeToken.Kind.LongOption, position, string.Empty, "--");
            return ParseExceptionFactory.Create(reader.Original, token, "Invalid long option name.", "Did you forget the option name?");
        }

        internal static ParseException LongOptionNameIsOneCharacter(TextBuffer reader, int position, string name)
        {
            var token = new CommandTreeToken(CommandTreeToken.Kind.LongOption, position, name, $"--{name}");
            var reason = $"Did you mean -{name}?";
            return ParseExceptionFactory.Create(reader.Original, token, "Invalid long option name.", reason);
        }

        internal static ParseException LongOptionNameStartWithDigit(TextBuffer reader, int position, string name)
        {
            var token = new CommandTreeToken(CommandTreeToken.Kind.LongOption, position, name, $"--{name}");
            return ParseExceptionFactory.Create(reader.Original, token, "Invalid long option name.", "Option names cannot start with a digit.");
        }

        internal static ParseException LongOptionNameContainSymbol(TextBuffer reader, int position, char character)
        {
            var name = character.ToString(CultureInfo.InvariantCulture);
            var token = new CommandTreeToken(CommandTreeToken.Kind.LongOption, position, name, name);
            return ParseExceptionFactory.Create(reader.Original, token, "Invalid long option name.", "Invalid character.");
        }

        internal static ParseException UnterminatedQuote(string input, CommandTreeToken token)
        {
            return ParseExceptionFactory.Create(input, token, $"Encountered unterminated quoted string '{token.Value}'.", "Did you forget the closing quotation mark?");
        }

        internal static ParseException UnknownCommand(CommandModel model, CommandTree? node, IEnumerable<string> args, CommandTreeToken token)
        {
            var suggestion = CommandSuggestor.Suggest(model, node?.Command, token.Value);
            var text = suggestion != null ? $"Did you mean '{suggestion.Name}'?" : "No such command.";
            return ParseExceptionFactory.Create(args, token, $"Unknown command '{token.Value}'.", text);
        }

        internal static ParseException CouldNotMatchArgument(IEnumerable<string> args, CommandTreeToken token)
        {
            return ParseExceptionFactory.Create(args, token, $"Could not match '{token.Value}' with an argument.", "Could not match to argument.");
        }

        internal static ParseException UnknownOption(IEnumerable<string> args, CommandTreeToken token)
        {
            return ParseExceptionFactory.Create(args, token, $"Unknown option '{token.Value}'.", "Unknown option.");
        }

        internal static ParseException ValueIsNotInValidFormat(string value)
        {
            var text = $"[red]Error:[/] The value '[white]{value}[/]' is not in a correct format";
            return new ParseException("Could not parse value", Text.Markup(text));
        }
    }
}
