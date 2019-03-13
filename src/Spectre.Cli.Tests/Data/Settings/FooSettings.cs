using System.ComponentModel;

namespace Spectre.Cli.Tests.Data.Settings
{
    public class FooCommandSettings : CommandSettings
    {
        [CommandArgument(0, "[QUX]")]
        [Description("The qux value.")]
        public string Qux { get; set; }
    }
}
