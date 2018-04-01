namespace Spectre.CommandLine.Tests.Data
{
    public class MammalSettings : AnimalSettings
    {
        [CommandOption("-n|--name <VALUE>")]
        public string Name { get; set; }
    }
}