using System.Collections.Generic;

// ReSharper disable once CheckNamespace
namespace Spectre.CommandLine.Internal
{
    internal interface ICommandContainer
    {
        ICollection<CommandDefinition> Commands { get; }
    }
}
