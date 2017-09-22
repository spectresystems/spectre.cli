// Licensed to Spectre Systems AB under one or more agreements.
// Spectre Systems AB licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.ComponentModel;
using System.Reflection;
using Spectre.CommandLine.Annotations;

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

        private ParameterInfo(
            Type type, ParameterType parameterType, PropertyInfo property, string description,
            bool isInherited, TypeConverterAttribute converter)
        {
            Type = type;
            ParameterType = parameterType;
            Property = property;
            Description = description;
            IsInherited = isInherited;
            Converter = converter;
        }

        public static ParameterInfo Create(CommandInfo command, PropertyInfo property)
        {
            var description = property.GetCustomAttribute<DescriptionAttribute>()?.Description;
            var converter = property.GetCustomAttribute<TypeConverterAttribute>();
            var inherited = property.DeclaringType != command.SettingsType;

            var type = property.PropertyType == typeof(bool)
                ? ParameterType.Flag : ParameterType.Single;

            return new ParameterInfo(property.PropertyType, type, property, description, inherited, converter);
        }
    }
}