using System;
using System.ComponentModel;
using System.Globalization;

namespace Spectre.CommandLine.Tests.Converters
{
    public sealed class LengthTypeConverter : TypeConverter
    {
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string text)
            {
                return text.Length;
            }
            throw new NotSupportedException("Cannot convert the specified value.");
        }
    }
}
