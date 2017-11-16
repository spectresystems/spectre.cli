using System;
using System.ComponentModel;
using System.Reflection;

namespace Spectre.CommandLine.Internal.Modelling
{
    internal sealed class CommandArgument : CommandParameter
    {
        public string Value { get; }
        public int Position { get; set; }

        public CommandArgument(
            Type parameterType, ParameterKind parameterKind, PropertyInfo property, string description,
            TypeConverterAttribute converter, CommandArgumentAttribute argumentAttribute)
                : base(parameterType, parameterKind, property, description, converter, argumentAttribute.IsRequired)
        {
            Value = argumentAttribute.Value;
            Position = argumentAttribute.Position;
        }
    }
}
