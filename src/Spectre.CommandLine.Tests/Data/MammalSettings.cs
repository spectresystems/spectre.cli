namespace Spectre.CommandLine.Tests.Data
{
    public class MammalSettings : AnimalSettings
    {
        [CommandOption("--name <VALUE>")]
        public string Name { get; set; }
    }
}