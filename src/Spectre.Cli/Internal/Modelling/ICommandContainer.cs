using System.Collections.Generic;

namespace Spectre.Cli.Internal.Modelling
{
    internal interface ICommandContainer
    {
        IList<CommandInfo> Commands { get; }
    }
}
