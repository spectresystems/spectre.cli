using System.Collections.Generic;

namespace Spectre.Cli.Internal
{
    internal interface ICommandContainer
    {
        IList<CommandInfo> Commands { get; }
    }
}
