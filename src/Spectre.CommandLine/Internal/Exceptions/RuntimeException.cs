using System;
using Spectre.CommandLine.Internal.Modelling;
using Spectre.CommandLine.Internal.Parsing;

namespace Spectre.CommandLine.Internal.Exceptions
{
    internal class RuntimeException : CommandAppException
    {
        public RuntimeException(string message)
            : base(message)
        {
        }

        public RuntimeException(string message, Exception ex)
            : base(message, ex)
        {
        }

        public static RuntimeException CouldNotResolveType(Type type, Exception ex = null)
        {
            var message = $"Could not resolve type '{type.FullName}'.";
            if (ex != null)
            {
                return new RuntimeException(message, ex);
            }
            return new RuntimeException(message);
        }

        public static RuntimeException MissingRequiredArgument(CommandTree node, CommandArgument argument)
        {
            return new RuntimeException($"Command '{node.Command.Name}' is missing required argument '{argument.Value}'.");
        }

        public static RuntimeException ValidationFailed(ValidationResult result)
        {
            return new RuntimeException(result.Message);
        }
    }
}
