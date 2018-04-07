namespace Spectre.CommandLine.Tests.Data.Settings
{
    public class MammalSettings : AnimalSettings
    {
        [CommandOption("-n|--name <VALUE>")]
        public string Name { get; set; }
    }
}