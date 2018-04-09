using System;
using Spectre.Cli.Internal.Modelling;
using Spectre.Cli.Internal.Parsing;
using Spectre.Cli.Internal.Rendering;

namespace Spectre.Cli.Internal.Exceptions
{
    internal class RuntimeException : CommandAppException
    {
        public RuntimeException(string message, IRenderable pretty = null)
            : base(message, pretty)
        {
        }

        public RuntimeException(string message, Exception ex, IRenderable pretty = null)
            : base(message, ex, pretty)
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
