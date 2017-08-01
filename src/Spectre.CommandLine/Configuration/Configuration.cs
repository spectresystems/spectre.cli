using System.Collections.Generic;
using Spectre.CommandLine.Annotations;

namespace Spectre.CommandLine.Configuration
{
    internal sealed class Configuration : IConfiguration
    {
        public string ApplicationName { get; set; }
        public OptionAttribute Help { get; set; }

        public ICollection<CommandInfo> Commands { get; }

        public Configuration()
        {
            Commands = new List<CommandInfo>();
            Help = new OptionAttribute("-h|--help");
        }
    }
}
