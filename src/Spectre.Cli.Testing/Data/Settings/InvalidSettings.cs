namespace Spectre.Cli.Testing.Data.Settings
{
    public sealed class InvalidSettings : CommandSettings
    {
        [CommandOption("-f|--foo [BAR]")]
        public string Value { get; set; }
    }
}
