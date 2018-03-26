using System;
using System.Collections.Generic;
using System.Linq;
using Spectre.CommandLine.Internal.Modelling;
using Spectre.CommandLine.Internal.Parsing.Tokenization;

namespace Spectre.CommandLine.Internal.Parsing
{
    internal class CommandTreeParser
    {
        private readonly CommandModel _configuration;
        private readonly CommandOptionAttribute _help;

        public CommandTreeParser(CommandModel configuration, CommandOptionAttribute help = null)
        {
            _configuration = configuration;
            _help = help;
        }

        public (CommandTree tree, ILookup<string, string> remaining) Parse(IEnumerable<string> args)
        {
            var tokens = CommandTreeTokenizer.Tokenize(args);
            var context = new CommandTreeParserContext();

            var result = (CommandTree)null;
            if (tokens.Count > 0)
            {
                var token = tokens.Current;
                if (token.TokenKind != CommandTreeToken.Kind.String)
                {
                    if (_help != null)
                    {
                        if (_help.ShortName?.Equals(token.Value, StringComparison.Ordinal) == true ||
                            _help.LongName?.Equals(token.Value, StringComparison.Ordinal) == true)
                        {
                            // Show help
                            return (null, context.GetRemainingArguments());
                        }
                    }
                    throw new CommandAppException($"Unexpected option '{token.Value}'.");
                }
                result = ParseCommand(context, _configuration, null, tokens);
            }

            return (result, context.GetRemainingArguments());
        }

        private CommandTree ParseCommand(
            CommandTreeParserContext context,
            ICommandContainer current,
            CommandTree parent,
            CommandTreeTokenStream stream)
        {
            context.ResetArgumentPosition();

            // Find the command.
            var commandToken = stream.Consume(CommandTreeToken.Kind.String);
            var command = current.FindCommand(commandToken.Value);
            if (command == null)
            {
                throw new CommandAppException($"Unknown command '{commandToken.Value}'.");
            }

            var node = new CommandTree(parent, command);
            while (stream.Peek() != null)
            {
                var token = stream.Peek();
                switch (token.TokenKind)
                {
                    case CommandTreeToken.Kind.LongOption:
                        // Long option
                        ParseOption(context, stream, node, true);
                        break;
                    case CommandTreeToken.Kind.ShortOption:
                        // Short option
                        ParseOption(context, stream, node, false);
                        break;
                    case CommandTreeToken.Kind.String:
                        // Command
                        ParseString(context, stream, node);
                        break;
                    default:
                        throw new InvalidOperationException("Unknown token type.");
                }
            }

            // Add unmapped parameters.
            foreach (var parameter in node.Command.Parameters)
            {
                if (node.Mapped.All(m => m.Item1 != parameter))
                {
                    node.Unmapped.Add(parameter);
                }
            }

            return node;
        }

        private void ParseString(
            CommandTreeParserContext context,
            CommandTreeTokenStream stream,
            CommandTree node)
        {
            var token = stream.Expect(CommandTreeToken.Kind.String);

            // Command?
            var command = node.Command.FindCommand(token.Value);
            if (command != null)
            {
                node.Next = ParseCommand(context, node.Command, node, stream);
                return;
            }

            // Current command has no arguments?
            if (!node.HasArguments())
            {
                throw new CommandAppException($"Unknown child command '{token.Value}' of '{node.Command.Name}'.");
            }

            // Argument?
            var parameter = node.FindArgument(context.CurrentArgumentPosition);
            if (parameter == null)
            {
                throw new CommandAppException($"Could not match '{token.Value}' with an argument.");
            }

            node.Mapped.Add((parameter, stream.Consume(CommandTreeToken.Kind.String).Value));

            context.IncreaseArgumentPosition();
        }

        private void ParseOption(
            CommandTreeParserContext context,
            CommandTreeTokenStream stream,
            CommandTree node,
            bool isLongOption)
        {
            // Get the option token.
            var token = stream.Consume(isLongOption ? CommandTreeToken.Kind.LongOption : CommandTreeToken.Kind.ShortOption);

            // Find the option.
            var option = node.FindOption(token.Value, isLongOption);
            if (option != null)
            {
                node.Mapped.Add((option, ParseParameterValue(stream, node, option)));
                return;
            }

            // Help?
            if (_help != null)
            {
                if (_help.ShortName?.Equals(token.Value, StringComparison.Ordinal) == true ||
                    _help.LongName?.Equals(token.Value, StringComparison.Ordinal) == true)
                {
                    node.ShowHelp = true;
                    return;
                }
            }

            // Parent does not have this option mapped?
            if (!node.IsMappedWithParent(token.Value, isLongOption))
            {
                // Add this option as an remaining argument.
                var optionName = token.TokenKind == CommandTreeToken.Kind.LongOption ? $"--{token.Value}" : $"-{token.Value}";
                context.AddRemainingArgument(optionName, ParseParameterValue(stream, node, null));
            }
        }

        private static string ParseParameterValue(CommandTreeTokenStream stream, CommandTree current, CommandParameter parameter)
        {
            var value = (string)null;

            // Parse the value of the token (if any).
            var valueToken = stream.Peek();
            if (valueToken?.TokenKind == CommandTreeToken.Kind.String)
            {
                // Is this a command?
                if (current.Command.FindCommand(valueToken.Value) == null)
                {
                    if (parameter != null)
                    {
                        if (parameter.ParameterKind == ParameterKind.Single)
                        {
                            value = stream.Consume(CommandTreeToken.Kind.String).Value;
                        }
                        else if (parameter.ParameterKind == ParameterKind.Flag)
                        {
                            throw new CommandAppException("Flags cannot be assigned a value.");
                        }
                    }
                    else
                    {
                        // Unknown parameter value.
                        value = stream.Consume(CommandTreeToken.Kind.String).Value;
                    }
                }
            }

            // No value?
            if (value == null && parameter != null)
            {
                if (parameter.ParameterKind == ParameterKind.Flag)
                {
                    value = "true";
                }
                else
                {
                    switch (parameter)
                    {
                        case CommandOption option:
                            throw new CommandAppException($"Option '{option.GetOptionName()}' is defined but no value has been provided.");
                        case CommandArgument argument:
                            throw new CommandAppException($"Argument '{argument.Value}' is defined but no value has been provided.");
                    }
                }
            }

            return value;
        }
    }
}
