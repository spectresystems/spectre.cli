using System.ComponentModel;

namespace Spectre.Cli.Tests.Data.Settings
{
    public class BarCommandSettings : FooCommandSettings
    {
        [CommandArgument(0, "<CORGI>")]
        [Description("The corgi value.")]
        public string Corgi { get; set; }
    }
}
