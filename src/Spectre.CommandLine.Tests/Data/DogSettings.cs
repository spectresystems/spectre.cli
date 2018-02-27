namespace Spectre.CommandLine.Tests.Data
{
    public class DogSettings : MammalSettings
    {
        [CommandArgument(0, "<AGE>")]
        public int Age { get; set; }

        [CommandOption("-g|--good-boy")]
        public bool GoodBoy { get; set; }
    }
}