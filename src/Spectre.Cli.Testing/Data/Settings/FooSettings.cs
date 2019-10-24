using System.ComponentModel;

namespace Spectre.Cli.Testing.Data.Settings
{
    public class FooCommandSettings : CommandSettings
    {
        [CommandArgument(0, "[QUX]")]
        [Description("The qux value.")]
        public string Qux { get; set; }
    }
}
