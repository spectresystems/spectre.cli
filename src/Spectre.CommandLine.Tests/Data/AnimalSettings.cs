using System.ComponentModel;

namespace Spectre.CommandLine.Tests.Data
{
    public class AnimalSettings
    {
        [CommandOption("-a|--alive [VALUE]")]
        [Description("Indicates whether or not the animal is alive.")]
        public bool IsAlive { get; set; }
    }
}