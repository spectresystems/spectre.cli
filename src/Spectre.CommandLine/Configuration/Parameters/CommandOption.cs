using System.ComponentModel;
using System.Reflection;
using Spectre.CommandLine.Annotations;

namespace Spectre.CommandLine.Configuration.Parameters
{
    internal sealed class CommandOption : CommandParameter
    {
        public string LongName { get; }
        public string ShortName { get; }
        public string ValueName { get; }
        public DefaultValueAttribute DefaultValue { get; }

        public CommandOption(ParameterInfo info, OptionAttribute attribute, DefaultValueAttribute defaultValue) 
            : base(info)
        {
            LongName = attribute.LongName;
            ShortName = attribute.ShortName;
            ValueName = attribute.ValueName;
            DefaultValue = defaultValue;
        }

        public string GetOptionName()
        {
            return !string.IsNullOrWhiteSpace(LongName)
                ? $"--{LongName}" : $"-{ShortName}";
        }

        public static CommandOption Create(ParameterInfo parameter, OptionAttribute attribute)
        {
            return new CommandOption(
                parameter,
                attribute,
                parameter.Property.GetCustomAttribute<DefaultValueAttribute>());
        }
    }
}