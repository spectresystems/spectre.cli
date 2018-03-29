using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Spectre.CommandLine.Internal.Modelling;
using Spectre.CommandLine.Internal.Parsing;
using Spectre.CommandLine.Internal.Parsing.Tokenization;

namespace Spectre.CommandLine.Internal
{
    internal static class ExceptionHelper
    {
        public static CommandAppException MissingRequiredArgument(CommandTree node, CommandArgument argument)
        {
            return new CommandAppException($"Command '{node.Command.Name}' is missing required argument '{argument.Value}'.");
        }

        public static ConfigurationException NoCommandConfigured()
        {
            return new ConfigurationException("No commands have been configured.");
        }

        public static ConfigurationException CouldNotResolveType(Type type, Exception ex = null)
        {
            var message = $"Could not resolve type '{type.FullName}'.";
            if (ex != null)
            {
                return new ConfigurationException(message, ex);
            }
            return new ConfigurationException(message);
        }

        public static CommandAppException ValidationFailed(ValidationResult result)
        {
            return new CommandAppException(result.Message);
        }

        public static class Model
        {
            public static class Validation
            {
                public static ConfigurationException DuplicateOption(CommandInfo command, string[] options)
                {
                    var keys = string.Join(", ", options.Select(x => x.Length > 1 ? $"--{x}" : $"-{x}"));
                    if (options.Length > 1)
                    {
                        return new ConfigurationException($"Options {keys} are duplicated in command '{command.Name}'.");
                    }

                    return new ConfigurationException($"Option {keys} is duplicated in command '{command.Name}'.");
                }
            }
        }

        public static class Tree
        {
            public static CommandAppException CouldNotCreateSettings(Type settingsType)
            {
                return new CommandAppException($"Could not create settings of type '{settingsType.FullName}'.");
            }

            public static CommandAppException CouldNotCreateCommand(Type commandType)
            {
                return new CommandAppException($"Could not create command of type '{commandType.FullName}'.");
            }

            public static class Tokenization
            {
                public static CommandAppException UnterminatedQuote(string quote)
                {
                    return new CommandAppException($"Encountered unterminated quote '{quote}'.");
                }

                public static CommandAppException UnterminatedOption()
                {
                    return new CommandAppException("Encountered unterminated option.");
                }

                public static CommandAppException OptionWithoutName()
                {
                    return new CommandAppException("Option does not have a name.");
                }

                public static CommandAppException OptionWithoutValidName()
                {
                    return new CommandAppException("Option does not have a valid name.");
                }

                public static CommandAppException ExpectedTokenButFoundNull(CommandTreeToken.Kind expected)
                {
                    var message = $"Expected to find any token of type '{expected}' but found null instead.";
                    return new CommandAppException(message);
                }

                public static CommandAppException ExpectedTokenButFoundOther(CommandTreeToken.Kind expected, CommandTreeToken.Kind found)
                {
                    var message = $"Expected to find token of type '{expected}' but found '{found}' instead.";
                    return new CommandAppException(message);
                }
            }

            public static class Parsing
            {
                public static CommandAppException UnexpectedOption(CommandTreeToken token)
                {
                    return new CommandAppException($"Unexpected option '{token.Value}'.");
                }

                public static CommandAppException UnknownCommand(CommandTreeToken token)
                {
                    return new CommandAppException($"Unknown command '{token.Value}'.");
                }

                public static IndexOutOfRangeException UnknownTokenKind(CommandTreeToken.Kind kind)
                {
                    // Internal exception. Shouldn't really happen.
                    return new IndexOutOfRangeException($"Encountered unknown token kind ('{kind}').");
                }

                public static CommandAppException CouldNotMatchArgument(CommandTreeToken token)
                {
                    return new CommandAppException($"Could not match '{token.Value}' with an argument.");
                }

                public static CommandAppException CannotAssignValueToFlag()
                {
                    return new CommandAppException("Flags cannot be assigned a value.");
                }

                public static CommandAppException NoValueForOption(CommandOption option)
                {
                    return new CommandAppException($"Option '{option.GetOptionName()}' is defined but no value has been provided.");
                }

                public static CommandAppException NoValueForArgument(CommandArgument argument)
                {
                    return new CommandAppException($"Argument '{argument.Value}' is defined but no value has been provided.");
                }
            }
        }

        public static class Template
        {
            public static class Tokenization
            {
                public static ConfigurationException UnexpectedToken(char token)
                {
                    return new ConfigurationException($"Encountered unexpected token '{token}'.");
                }

                public static ConfigurationException UnterminatedValueName(string name)
                {
                    return new ConfigurationException($"Encountered unterminated value name '{name}'.");
                }
            }

            public static class Parsing
            {
                public static ConfigurationException ArgumentCannotContainOptions()
                {
                    return new ConfigurationException("Arguments can not contain options.");
                }

                public static ConfigurationException MultipleValuesAreNotSupported()
                {
                    return new ConfigurationException("Multiple values are not supported.");
                }

                public static ConfigurationException ValuesMustHaveName()
                {
                    return new ConfigurationException("Values without name are not allowed.");
                }

                public static class Options
                {
                    public static ConfigurationException OptionsMustHaveName()
                    {
                        return new ConfigurationException("Options without name are not allowed.");
                    }

                    public static ConfigurationException OptionNamesCannotStartWithDigit()
                    {
                        return new ConfigurationException("Option names cannot start with a digit.");
                    }

                    public static ConfigurationException InvalidCharacterInOptionName(char character)
                    {
                        return new ConfigurationException($"Encountered invalid character '{character}' in option name.");
                    }

                    public static ConfigurationException MultipleLongOptionNamesNotAllowed()
                    {
                        return new ConfigurationException("Multiple long option names are not supported.");
                    }

                    public static ConfigurationException LongOptionMustHaveMoreThanOneCharacter()
                    {
                        return new ConfigurationException("Long option names must consist of more than one character.");
                    }

                    public static ConfigurationException MultipleShortOptionNamesNotAllowed()
                    {
                        return new ConfigurationException("Multiple short option names are not supported.");
                    }

                    public static ConfigurationException ShortOptionMustOnlyBeOneCharacter()
                    {
                        return new ConfigurationException("Short option names can not be longer than one character.");
                    }

                    public static ConfigurationException MultipleOptionValuesAreNotSupported()
                    {
                        return new ConfigurationException("Multiple option values are not supported.");
                    }

                    public static ConfigurationException OptionValueCannotBeOptional()
                    {
                        return new ConfigurationException("Option values cannot be optional.");
                    }

                    public static ConfigurationException InvalidCharacterInValueName(char character)
                    {
                        return new ConfigurationException($"Encountered invalid character '{character}' in value name.");
                    }

                    public static ConfigurationException MissingLongAndShortName()
                    {
                        return new ConfigurationException("No long or short name for option has been specified.");
                    }
                }
            }
        }
    }
}
