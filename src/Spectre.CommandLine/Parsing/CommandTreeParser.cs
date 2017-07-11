using System;
using System.Linq;
using Spectre.CommandLine.Configuration;
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
            return Parse(_configuration, tokens);
        }

        private CommandTree Parse(ICommandContainer commands, TokenStream stream)
        {
            if (stream.Count == 0)
            {
                return null;
            }
            return ParseCommand(commands, null, stream);
        }

        private CommandTree ParseCommand(
            ICommandContainer current,
            CommandTree parent,
            TokenStream stream)
        {
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
                        node.Next = ParseCommand(node.Command, node, stream);
                        break;
                    default:
                        throw new NotImplementedException("Unknown token type.");
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

        private void ParseOption(TokenStream stream, CommandTree node, bool isLongOption)
        {
            // Get the option token.
            var token = stream.Consume(isLongOption ? Token.Type.LongOption : Token.Type.ShortOption);

            // Find the option.
            var option = FindOption(node, token, isLongOption);
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

        private static CommandOption FindOption(CommandTree node, Token token, bool isLongOption)
        {
            var current = node;
            while (current != null)
            {
                var option = current.Command.Parameters.OfType<CommandOption>()
                    .FirstOrDefault(o => isLongOption ? o.LongName == token.Value : o.ShortName == token.Value);

                if (option != null)
                {
                    return option;
                }

                current = current.Parent;
            }
            return null;
        }

        private static string ParseParameterValue(TokenStream stream, CommandTree current, CommandOption parameter)
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
                        if (parameter.Info.ParameterType == ParameterType.Single)
                        {
                            value = stream.Consume(Token.Type.String).Value;
                        }
                        else if (parameter.Info.ParameterType == ParameterType.Flag)
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
                if (parameter.Info.ParameterType == ParameterType.Flag)
                {
                    value = true.ToString();
                }
                else
                {
                    throw new CommandAppException($"Option '{parameter.GetOptionName()}' is defined but no value has been provided.");
                }
            }

            return value;
        }
    }
}
