using System;
using System.Linq;

namespace Spectre.CommandLine.Internal.Modelling
{
    internal static class CommandContainerExtensions
    {
        public static CommandInfo FindCommand(this ICommandContainer root, string name)
        {
            return root.Commands.FirstOrDefault(c => c.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }
    }
}
