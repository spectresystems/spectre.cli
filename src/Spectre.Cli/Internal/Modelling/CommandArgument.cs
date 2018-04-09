using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace Spectre.Cli.Internal.Modelling
{
    internal sealed class CommandArgument : CommandParameter
    {
        public string Value { get; }
        public int Position { get; set; }

        public CommandArgument(
            Type parameterType, ParameterKind parameterKind, PropertyInfo property, string description,
            TypeConverterAttribute converter, CommandArgumentAttribute argument, IEnumerable<ParameterValidationAttribute> validators)
                : base(parameterType, parameterKind, property, description, converter, validators, argument.IsRequired)
        {
            Value = argument.ValueName;
            Position = argument.Position;
        }
    }
}
