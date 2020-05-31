using System;
using System.Linq;

namespace Spectre.Cli.Internal
{
    internal static class CommandContainerExtensions
    {
        public static CommandInfo FindCommand(this ICommandContainer root, string name)
        {
            var result = root.Commands.FirstOrDefault(c => c.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            if (result == null)
            {
                result = root.Commands.FirstOrDefault(c => c.Aliases.Contains(name, StringComparer.OrdinalIgnoreCase));
            }

            return result;
        }
    }
}
