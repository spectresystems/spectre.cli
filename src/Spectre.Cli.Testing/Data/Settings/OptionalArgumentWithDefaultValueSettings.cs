using System.ComponentModel;

namespace Spectre.Cli.Testing.Data.Settings
{
    public sealed class OptionalArgumentWithDefaultValueSettings : CommandSettings
    {
        [CommandArgument(0, "[GREETING]")]
        [DefaultValue("Hello World")]
        public string Greeting { get; set; }
    }

    public sealed class RequiredArgumentWithDefaultValueSettings : CommandSettings
    {
        [CommandArgument(0, "<GREETING>")]
        [DefaultValue("Hello World")]
        public string Greeting { get; set; }
    }
}
