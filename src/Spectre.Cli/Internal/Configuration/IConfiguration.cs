using System.Collections.Generic;

namespace Spectre.Cli.Internal.Configuration
{
    internal interface IConfiguration
    {
        IList<ConfiguredCommand> Commands { get; }
        CommandAppSettings Settings { get; }
        ConfiguredCommand? DefaultCommand { get; }
        IList<string[]> Examples { get; }
    }
}