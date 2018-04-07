using System.ComponentModel;
using System.Linq;
using Spectre.CommandLine.Tests.Data.Settings;

namespace Spectre.CommandLine.Tests.Data
{
    [Description("The dog command.")]
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
}