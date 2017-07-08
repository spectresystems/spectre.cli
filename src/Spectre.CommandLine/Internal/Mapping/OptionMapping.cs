using System;
using System.ComponentModel;
using System.Reflection;
using Microsoft.Extensions.CommandLineUtils;

// ReSharper disable once CheckNamespace
namespace Spectre.CommandLine.Internal
{
    internal sealed class OptionMapping : IMapping
    {
        public PropertyInfo Property { get; }
        public CommandOption Option { get; }
        public MappingType Kind { get; }

        public Type Type => Property.PropertyType;
        public bool HasValue => Option.HasValue();
        public string Value => Option.Value();
        public bool Required => false;

        public bool HasDefaultValue { get; }
        public object DefaultValue { get; }

        public TypeConverter Converter { get; }

        public OptionMapping(OptionDefinition definition, CommandOption option)
        {
            Property = definition.Property;
            Kind = definition.MappingType;
            Option = option;
            HasDefaultValue = definition.HasDefaultValue;
            DefaultValue = definition.DefaultValue;
            Converter = definition.Converter;
        }

        public void Assign(object obj, object value)
        {
            Property.SetValue(obj, value);
        }
    }
}