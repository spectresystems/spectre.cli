using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Spectre.CommandLine.Internal.Configuration;

namespace Spectre.CommandLine.Internal.Modelling
{
    internal static class CommandModelBuilder
    {
        public static CommandModel Build(IConfiguration configuration)
        {
            var result = new List<CommandInfo>();
            foreach (var command in configuration.Commands)
            {
                result.Add(Build(null, command));
            }

            var model = new CommandModel(configuration.ApplicationName, result);
            CommandModelValidator.Validate(model);
            return model;
        }

        private static CommandInfo Build(CommandInfo parent, ConfiguredCommand command)
        {
            var info = new CommandInfo(parent, command);

            if (!info.IsProxy)
            {
                var description = info.CommandType.GetCustomAttribute<DescriptionAttribute>();
                if (description != null)
                {
                    info.Description = description.Description;
                }
            }

            foreach (var parameter in GetParameters(info))
            {
                info.Parameters.Add(parameter);
            }

            foreach (var childCommand in command.Children)
            {
                var child = Build(info, childCommand);
                info.Children.Add(child);
            }

            // Normalize argument positions.
            var index = 0;
            foreach (var argument in info.Parameters.OfType<CommandArgument>().OrderBy(argument => argument.Position))
            {
                argument.Position = index;
                index++;
            }

            return info;
        }

        private static IEnumerable<CommandParameter> GetParameters(CommandInfo command)
        {
            foreach (var property in command.SettingsType.GetProperties())
            {
                if (property.IsDefined(typeof(CommandOptionAttribute)))
                {
                    var attribute = property.GetCustomAttribute<CommandOptionAttribute>();
                    if (attribute != null)
                    {
                        var option = BuildOptionParameter(property, attribute);

                        // Any previous command has this option defined?
                        if (command.HaveParentWithOption(option))
                        {
                            // Do we allow it to exist on this command as well?
                            if (command.AllowParentOption(option))
                            {
                                option.Required = false;
                                option.IsShadowed = true;
                                yield return option;
                            }
                        }
                        else
                        {
                            // No parent have this option.
                            yield return option;
                        }
                    }
                }
                else if (property.IsDefined(typeof(CommandArgumentAttribute)))
                {
                    var attribute = property.GetCustomAttribute<CommandArgumentAttribute>();
                    if (attribute != null)
                    {
                        var argument = BuildArgumentParameter(property, attribute);

                        // Any previous command has this argument defined?
                        // In that case, we should not assign the parameter to this command.
                        if (!command.HaveParentWithArgument(argument))
                        {
                            yield return argument;
                        }
                    }
                }
            }
        }

        private static CommandOption BuildOptionParameter(PropertyInfo property, CommandOptionAttribute attribute)
        {
            var description = property.GetCustomAttribute<DescriptionAttribute>();
            var converter = property.GetCustomAttribute<TypeConverterAttribute>();
            var defaultValue = property.GetCustomAttribute<DefaultValueAttribute>();

            var kind = property.PropertyType == typeof(bool)
                ? ParameterKind.Flag
                : ParameterKind.Single;

            return new CommandOption(property.PropertyType, kind,
                property, description?.Description, converter,
                attribute, defaultValue);
        }

        private static CommandArgument BuildArgumentParameter(PropertyInfo property, CommandArgumentAttribute attribute)
        {
            var description = property.GetCustomAttribute<DescriptionAttribute>();
            var converter = property.GetCustomAttribute<TypeConverterAttribute>();

            var kind = property.PropertyType == typeof(bool)
                ? ParameterKind.Flag
                : ParameterKind.Single;

            return new CommandArgument(property.PropertyType, kind,
                property, description?.Description, converter, attribute);
        }
    }
}
