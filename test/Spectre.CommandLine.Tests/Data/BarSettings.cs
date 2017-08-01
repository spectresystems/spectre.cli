using System.ComponentModel;
using Spectre.CommandLine.Annotations;
using Spectre.CommandLine.Tests.Converters;

namespace Spectre.CommandLine.Tests.Data
{
    public class BarSettings : FooSettings
    {
        [Option("-b|--bar")]
        [DefaultValue(3)]
        public int Bar { get; set; }

        [Option("-q|--qux")]
        [TypeConverter(typeof(LengthTypeConverter))]
        public int Qux { get; set; }
    }
}