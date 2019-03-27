namespace Spectre.Cli.Tests.Data.Settings
{
    public class ArgumentVectorSettings : CommandSettings
    {
        [CommandArgument(0, "<Foos>")]
        public string[] Foo { get; set; }
    }
}
