using System.Collections.Generic;

namespace Spectre.CommandLine.Internal.Configuration
{
    internal interface IConfiguration
    {
        IList<ConfiguredCommand> Commands { get; }
        string ApplicationName { get; }
    }
}