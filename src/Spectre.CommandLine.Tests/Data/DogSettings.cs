namespace Spectre.CommandLine.Tests.Data
{
    public class DogSettings : MammalSettings
    {
        [CommandOption("-g|--good-boy [VALUE]")]
        public bool GoodBoy { get; set; }

        [CommandArgument(0, "[AGE]")]
        public int Age { get; set; }
    }
}