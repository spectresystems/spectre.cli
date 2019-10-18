using System.Collections.Generic;

namespace Spectre.Cli.Internal.Configuration
{
    internal interface IConfiguration
    {
        IList<ConfiguredCommand> Commands { get; }
        ConfigurationSettings Settings { get; }
        ConfiguredCommand? DefaultCommand { get; }
        IList<string[]> Examples { get; }
    }
}