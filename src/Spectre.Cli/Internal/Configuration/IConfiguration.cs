using System.Collections.Generic;

namespace Spectre.Cli.Internal.Configuration
{
    internal interface IConfiguration
    {
        IList<ConfiguredCommand> Commands { get; }
        string ApplicationName { get; }
    }
}