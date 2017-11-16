using System;
using System.ComponentModel;
using System.Reflection;

namespace Spectre.CommandLine.Internal.Modelling
{
    internal abstract class CommandParameter
    {
        public Type ParameterType { get; }
        public ParameterKind ParameterKind { get; }
        public PropertyInfo Property { get; }
        public string Description { get; }
        public TypeConverterAttribute Converter { get; }
        public bool Required { get; set; }

        protected CommandParameter(
            Type parameterType, ParameterKind parameterKind, PropertyInfo property,
            string description, TypeConverterAttribute converter, bool required)
        {
            ParameterType = parameterType;
            ParameterKind = parameterKind;
            Property = property;
            Description = description;
            Converter = converter;
            Required = required;
        }

        public bool HaveSameBackingPropertyAs(CommandParameter other)
        {
            return CommandParameterComparer.ByBackingProperty.Equals(this, other);
        }

        public void Assign(object settings, object value)
        {
            Property.SetValue(settings, value);
        }
    }
}