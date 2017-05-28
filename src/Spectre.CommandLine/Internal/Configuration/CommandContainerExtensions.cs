using System;
using System.Linq;

// ReSharper disable once CheckNamespace
namespace Spectre.CommandLine.Internal
{
    internal static class CommandContainerExtensions
    {
        public static CommandDefinition Find(this ICommandContainer container, string name)
        {
            return container.Commands.FirstOrDefault(c => c.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }
    }
}
