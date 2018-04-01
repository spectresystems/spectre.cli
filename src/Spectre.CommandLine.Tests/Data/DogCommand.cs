using System.Linq;

namespace Spectre.CommandLine.Tests.Data
{
    public class DogCommand : AnimalCommand<DogSettings>
    {
        public override ValidationResult Validate(DogSettings settings, ILookup<string, string> remaining)
        {
            if (settings.Age > 100 && !remaining.Contains("zombie"))
            {
                return ValidationResult.Error("Dog is too old...");
            }
            return base.Validate(settings, remaining);
        }

        public override int Execute(DogSettings settings, ILookup<string, string> remaining)
        {
            DumpSettings(settings, remaining);
            return 0;
        }
    }

    public sealed class DogSettings : MammalSettings
    {
        [CommandArgument(0, "<AGE>")]
        public int Age { get; set; }

        [CommandOption("-g|--good-boy")]
        public bool GoodBoy { get; set; }

        public override ValidationResult Validate()
        {
            if (Name == "Tiger")
            {
                return ValidationResult.Error("Tiger is not a dog name!");
            }
            return ValidationResult.Success();
        }
    }
}