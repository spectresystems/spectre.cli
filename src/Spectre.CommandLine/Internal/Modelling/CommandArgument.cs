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
            TypeConverterAttribute converter, CommandArgumentAttribute argument)
                : base(parameterType, parameterKind, property, description, converter, argument.IsRequired)
        {
            Value = argument.Value;
            Position = argument.Position;
        }
    }
}
