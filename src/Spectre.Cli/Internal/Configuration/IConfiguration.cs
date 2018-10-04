using System.Collections.Generic;

namespace Spectre.Cli.Internal.Configuration
{
    internal interface IConfiguration
    {
        ConfiguredCommand DefaultCommand { get; }
        IList<ConfiguredCommand> Commands { get; }
        string ApplicationName { get; }
        ParsingMode ParsingMode { get; }
    }
}