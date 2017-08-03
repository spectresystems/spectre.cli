using System;
using System.Linq;
using Spectre.CommandLine.Configuration;
using Spectre.CommandLine.Configuration.Parameters;
using Spectre.CommandLine.Parsing.Tokenization;

namespace Spectre.CommandLine.Parsing
{
    internal class CommandTreeParser
    {
        private readonly IConfiguration _configuration;

        public CommandTreeParser(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public CommandTree Parse(string[] args)
        {
            var tokens = Tokenizer.Tokenize(args);
            return Parse(new CommandTreeParserContext(), _configuration, tokens);
        }

        private CommandTree Parse(CommandTreeParserContext context, ICommandContainer commands, TokenStream stream)
        {
            return stream.Count == 0 
                ? null : ParseCommand(context, commands, null, stream);
        }

        private CommandTree ParseCommand(
            CommandTreeParserContext context,
            ICommandContainer current,
            CommandTree parent,
            TokenStream stream)
        {
            context.ResetArgumentPosition();
            
            // Find the command.
            var commandToken = stream.Consume(Token.Type.String);
            var command = current.FindCommand(commandToken.Value);
            if (command == null)
            {
                throw new CommandAppException($"Unknown command '{commandToken.Value}'.");
            }

            var node = new CommandTree(parent, command);
            while (stream.Peek() != null)
            {
                var token = stream.Peek();
                switch (token.TokenType)
                {
                    case Token.Type.LongOption:
                        // Long option
                        ParseOption(stream, node, true);
                        break;
                    case Token.Type.ShortOption:
                        // Short option
                        ParseOption(stream, node, false);
                        break;
                    case Token.Type.String:
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
            TokenStream stream,
            CommandTree node)
        {
            var token = stream.Expect(Token.Type.String);

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

            node.Mapped.Add((parameter, stream.Consume(Token.Type.String).Value));

            context.IncreaseArgumentPosition();
        }

        private void ParseOption(TokenStream stream, CommandTree node, bool isLongOption)
        {
            // Get the option token.
            var token = stream.Consume(isLongOption ? Token.Type.LongOption : Token.Type.ShortOption);

            // Find the option.
            var option = node.FindOption(token.Value, isLongOption);
            if (option != null)
            {
                node.Mapped.Add((option, ParseParameterValue(stream, node, option)));
                return;
            }

            if (_configuration.Help != null)
            {
                // Help?
                if (_configuration.Help.ShortName != null && _configuration.Help.ShortName.Equals(token.Value, StringComparison.Ordinal) ||
                    _configuration.Help.LongName != null && _configuration.Help.LongName.Equals(token.Value, StringComparison.Ordinal))
                {
                    node.ShowHelp = true;
                    return;
                }
            }

            throw new CommandAppException($"Unknown option '{token.Value}'.");
        }

        private static string ParseParameterValue(TokenStream stream, CommandTree current, CommandParameter parameter)
        {
            var value = (string)null;

            // Parse the value of the token (if any).
            var valueToken = stream.Peek();
            if (valueToken != null && valueToken.TokenType == Token.Type.String)
            {
                // Is this a command?
                if (current.Command.FindCommand(valueToken.Value) == null)
                {
                    if (parameter != null)
                    {
                        if (parameter.Parameter.ParameterType == ParameterType.Single)
                        {
                            value = stream.Consume(Token.Type.String).Value;
                        }
                        else if (parameter.Parameter.ParameterType == ParameterType.Flag)
                        {
                            throw new CommandAppException("Flags cannot be assigned a value.");
                        }
                    }
                    else
                    {
                        // Unknown parameter value.
                        value = stream.Consume(Token.Type.String).Value;
                    }
                }
            }

            // No value?
            if (value == null && parameter != null)
            {
                if (parameter.Parameter.ParameterType == ParameterType.Flag)
                {
                    value = true.ToString();
                }
                else
                {
                    if (parameter is CommandOption option)
                    {
                        throw new CommandAppException($"Option '{option.GetOptionName()}' is defined but no value has been provided.");
                    }
                    if (parameter is CommandArgument argument)
                    {
                        throw new CommandAppException($"Argument '{argument.Name}' is defined but no value has been provided.");
                    }
                }
            }

            return value;
        }
    }
}
