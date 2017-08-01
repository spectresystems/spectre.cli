using System.ComponentModel;
using Spectre.CommandLine.Annotations;

namespace Sample.Commands.Settings
{
    public sealed class BarSettings : FooSettings
    {
        [Required]
        [Option("-b|--bar")]
        [Description("Triggers a bar.")]
        public string Bar { get; set; }
    }
}