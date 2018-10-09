namespace Spectre.Cli.Tests.Data.Settings
{
    public class MammalSettings : AnimalSettings
    {
        [CommandOption("-n|-p|--name|--pet-name <VALUE>")]
        public string Name { get; set; }
    }
}