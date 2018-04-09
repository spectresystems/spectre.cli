namespace Spectre.Cli.Tests.Data.Settings
{
    public class MammalSettings : AnimalSettings
    {
        [CommandOption("-n|--name <VALUE>")]
        public string Name { get; set; }
    }
}