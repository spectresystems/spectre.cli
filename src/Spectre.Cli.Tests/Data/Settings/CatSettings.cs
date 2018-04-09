using System.ComponentModel;
using Spectre.Cli.Tests.Data.Converters;
using Spectre.Cli.Tests.Data.Validators;

namespace Spectre.Cli.Tests.Data.Settings
{
    public class CatSettings : MammalSettings
    {
        [CommandOption("--agility <VALUE>")]
        [TypeConverter(typeof(CatAgilityConverter))]
        [DefaultValue(10)]
        [Description("The agility between 0 and 100.")]
        [PositiveNumberValidator("Agility cannot be negative.")]
        public int Agility { get; set; }
    }
}