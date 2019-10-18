using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace Spectre.Cli.Internal.Modelling
{
    internal abstract class CommandParameter
    {
        public Type ParameterType { get; }
        public ParameterKind ParameterKind { get; }
        public PropertyInfo Property { get; }
        public string? Description { get; }
        public TypeConverterAttribute? Converter { get; }
        public List<ParameterValidationAttribute> Validators { get; }
        public bool Required { get; set; }

        protected CommandParameter(
            Type parameterType, ParameterKind parameterKind, PropertyInfo property,
            string? description, TypeConverterAttribute? converter,
            IEnumerable<ParameterValidationAttribute> validators, bool required)
        {
            ParameterType = parameterType;
            ParameterKind = parameterKind;
            Property = property;
            Description = description;
            Converter = converter;
            Validators = new List<ParameterValidationAttribute>(validators ?? Array.Empty<ParameterValidationAttribute>());
            Required = required;
        }

        public bool HaveSameBackingPropertyAs(CommandParameter other)
        {
            return CommandParameterComparer.ByBackingProperty.Equals(this, other);
        }

        public void Assign(CommandSettings settings, object? value)
        {
            if (Property.PropertyType.IsArray)
            {
                // Add a new item to the array
                var array = (Array?)Property.GetValue(settings);
                Array newArray;

                var elementType = Property.PropertyType.GetElementType();
                if (elementType == null)
                {
                    throw new InvalidOperationException("Could not get property type.");
                }

                if (array == null)
                {
                    newArray = Array.CreateInstance(elementType, 1);
                }
                else
                {
                    newArray = Array.CreateInstance(elementType, array.Length + 1);
                    array.CopyTo(newArray, 0);
                }

                newArray.SetValue(value, newArray.Length - 1);
                value = newArray;
            }

            Property.SetValue(settings, value);
        }

        public object? Get(CommandSettings settings)
        {
            return Property.GetValue(settings);
        }
    }
}