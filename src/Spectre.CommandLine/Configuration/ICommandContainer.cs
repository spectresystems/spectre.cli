using System.Collections.Generic;

namespace Spectre.CommandLine.Configuration
{
    internal interface ICommandContainer
    {
        ICollection<CommandInfo> Commands { get; }
    }
}
