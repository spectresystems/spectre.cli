using System.ComponentModel;
using Spectre.CommandLine.Tests.Data.Converters;
using Spectre.CommandLine.Tests.Data.Validators;

namespace Spectre.CommandLine.Tests.Data
{
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