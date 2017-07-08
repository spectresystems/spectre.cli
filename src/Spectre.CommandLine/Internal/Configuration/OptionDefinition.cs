using System;
using System.ComponentModel;
using System.Reflection;

// ReSharper disable once CheckNamespace
namespace Spectre.CommandLine.Internal
{
    internal sealed class OptionDefinition
    {
        public Type Type { get; set; }

        public PropertyInfo Property { get; set; }

        public string Description { get; set; }

        public bool Inherited { get; set; }

        public string Template { get; set; }

        public MappingType MappingType { get; set; }

        public bool HasDefaultValue { get; set; }

        public object DefaultValue { get; set; }

        public TypeConverter Converter { get; set; }
    }
}