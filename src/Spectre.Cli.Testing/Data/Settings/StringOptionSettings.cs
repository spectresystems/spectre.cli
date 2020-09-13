namespace Spectre.Cli.Testing.Data.Settings
{
    public sealed class StringOptionSettings : CommandSettings
    {
        [CommandOption("-f|--foo")]
        public string Foo { get; set; }
    }
}
