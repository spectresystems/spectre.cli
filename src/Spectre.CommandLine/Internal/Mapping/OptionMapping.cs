using System;
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

        public bool HasDefaultValue { get; private set; }
        public object DefaultValue { get; private set; }

        public OptionMapping(PropertyInfo property, MappingType kind, CommandOption option)
        {
            Property = property;
            Kind = kind;
            Option = option;
        }

        public void SetDefaultValue(object value)
        {
            HasDefaultValue = true;
            DefaultValue = value;
        }

        public void Assign(object obj, object value)
        {
            Property.SetValue(obj, value);
        }
    }
}