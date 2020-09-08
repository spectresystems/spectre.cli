using System.ComponentModel;
using System.Globalization;

namespace Spectre.Cli.Testing.Data.Converters
{
    public sealed class StringToIntegerConverter : TypeConverter
    {
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string stringValue)
            {
                return int.Parse(stringValue, CultureInfo.InvariantCulture);
            }

            return base.ConvertFrom(context, culture, value);
        }
    }
}
