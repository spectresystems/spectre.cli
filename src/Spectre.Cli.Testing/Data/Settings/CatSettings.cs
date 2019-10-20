using System.ComponentModel;
using Spectre.Cli.Testing.Data.Converters;
using Spectre.Cli.Testing.Data.Validators;

namespace Spectre.Cli.Testing.Data.Settings
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