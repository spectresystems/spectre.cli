using Spectre.Cli.Internal;
using Spectre.Cli.Internal.Configuration;

namespace Spectre.Cli
{
    internal sealed class ConfigurationSettings
    {
        public string ApplicationName { get; set; }
        public ParsingMode ParsingMode { get; set; }
        public bool PropagateExceptions { get; set; }
        public bool ValidateExamples { get; set; }

        public ConfigurationSettings()
        {
            ParsingMode = ParsingMode.Relaxed;
        }
    }
}