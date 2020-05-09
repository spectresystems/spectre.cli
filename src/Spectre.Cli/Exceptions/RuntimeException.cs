using System;
using Spectre.Cli.Internal;
using Spectre.Cli.Internal.Modelling;
using Spectre.Cli.Internal.Parsing;
using Spectre.Cli.Internal.Rendering;

namespace Spectre.Cli.Exceptions
{
    /// <summary>
    /// Represents errors that occur during runtime.
    /// </summary>
    public class RuntimeException : CommandAppException
    {
        internal RuntimeException(string message, IRenderable? pretty = null)
            : base(message, pretty)
        {
        }

        internal RuntimeException(string message, Exception ex, IRenderable? pretty = null)
            : base(message, ex, pretty)
        {
        }

        internal static RuntimeException CouldNotResolveType(Type type, Exception? ex = null)
        {
            var message = $"Could not resolve type '{type.FullName}'.";
            if (ex != null)
            {
                return new RuntimeException(message, ex);
            }

            return new RuntimeException(message);
        }

        internal static RuntimeException MissingRequiredArgument(CommandTree node, CommandArgument argument)
        {
            if (node.Command.Name == Constants.DefaultCommandName)
            {
                return new RuntimeException($"Missing required argument '{argument.Value}'.");
            }

            return new RuntimeException($"Command '{node.Command.Name}' is missing required argument '{argument.Value}'.");
        }

        internal static RuntimeException NoConverterFound(CommandParameter parameter)
        {
            return new RuntimeException($"Could not find converter for type '{parameter.ParameterType.FullName}'.");
        }

        internal static RuntimeException ValidationFailed(ValidationResult result)
        {
            return new RuntimeException(result.Message ?? "Unknown validation error.");
        }

        internal static Exception CouldNotGetSettingsType(Type commandType)
        {
            return new RuntimeException($"Could not get settings type for command of type '{commandType.FullName}'.");
        }
    }
}
