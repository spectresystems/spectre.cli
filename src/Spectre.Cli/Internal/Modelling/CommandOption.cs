using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace Spectre.Cli.Internal.Modelling
{
    internal sealed class CommandOption : CommandParameter
    {
        public IReadOnlyList<string> LongNames { get; }
        public string ShortName { get; }
        public string ValueName { get; }
        public DefaultValueAttribute DefaultValue { get; }
        public bool IsShadowed { get; set; }

        public CommandOption(
            Type parameterType, ParameterKind parameterKind, PropertyInfo property, string description,
            TypeConverterAttribute converter, CommandOptionAttribute optionAttribute,
            IEnumerable<ParameterValidationAttribute> validators, DefaultValueAttribute defaultValue)
                : base(parameterType, parameterKind, property, description, converter, validators, false)
        {
            LongNames = optionAttribute.LongNames;
            ShortName = optionAttribute.ShortName;
            ValueName = optionAttribute.ValueName;
            DefaultValue = defaultValue;
        }

        public string GetOptionName()
        {
            return LongNames.Count > 0 ? LongNames[0] : ShortName;
        }
    }
}