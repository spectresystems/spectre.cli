using System.ComponentModel;
using System.Linq;
using Spectre.CommandLine.Tests.Data.Converters;
using Spectre.CommandLine.Tests.Data.Validators;

namespace Spectre.CommandLine.Tests.Data
{
    public class CatCommand : AnimalCommand<CatSettings>
    {
        public override int Execute(CatSettings settings, ILookup<string, string> remaining)
        {
            DumpSettings(settings, remaining);
            return 0;
        }
    }

    public sealed class CatSettings : MammalSettings
    {
        [CommandOption("--agility <VALUE>")]
        [TypeConverter(typeof(CatAgilityConverter))]
        [DefaultValue(10)]
        [Description("The option description.")]
        [PositiveNumberValidator("Agility cannot be negative.")]
        public int Agility { get; set; }
    }
}
