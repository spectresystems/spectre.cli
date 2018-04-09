namespace Spectre.Cli.Tests.Data.Settings
{
    public sealed class InvalidSettings : CommandSettings
    {
        [CommandOption("-f|--foo [BAR]")]
        public string Value { get; set; }
    }
}
