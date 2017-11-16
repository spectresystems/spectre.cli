using System.Collections.Generic;

namespace Spectre.CommandLine.Internal.Modelling
{
    internal interface ICommandContainer
    {
        IList<CommandInfo> Commands { get; }
    }
}
