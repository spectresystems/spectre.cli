using System;
using System.ComponentModel;
using System.Reflection;

namespace Spectre.CommandLine.Configuration
{
    internal sealed class ParameterInfo
    {
        public Type Type { get; }
        public ParameterType ParameterType { get; }
        public PropertyInfo Property { get; }
        public string Description { get; }
        public bool IsInherited { get; }
        public TypeConverterAttribute Converter { get; }
        public bool IsRequired { get; }

        public ParameterInfo(
            Type type, ParameterType parameterType, PropertyInfo property, string description,
            bool isInherited, TypeConverterAttribute converter, bool isRequired)
        {
            Type = type;
            ParameterType = parameterType;
            Property = property;
            Description = description;
            IsInherited = isInherited;
            Converter = converter;
            IsRequired = isRequired;
        }
    }
}