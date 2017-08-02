using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Spectre.CommandLine.Annotations;
using Spectre.CommandLine.Configuration.Parameters;

namespace Spectre.CommandLine.Configuration
{
    internal static class CommandInfoFactory
    {
        public static CommandInfo CreateProxy(CommandInfo parent, string name, Type settingsType)
        {
            var command = new CommandInfo(parent, name, null, settingsType);
            command.Parameters.AddRange(GetParameters(command));
            return command;
        }

        public static CommandInfo CreateCommand(CommandInfo parent, string name, Type commandType)
        {
            var settingsType = GetSettingsType(commandType);
            if (settingsType == null)
            {
                throw new InvalidOperationException($"Could not determine settings type for command of type '{name}'");
            }

            var command = new CommandInfo(parent, name, commandType, settingsType)
            {
                Description = commandType.GetTypeInfo().GetCustomAttribute<DescriptionAttribute>()?.Description
            };

            command.Parameters.AddRange(GetParameters(command));

            // Normalize argument positions.
            var index = 0;
            foreach (var argument in command.Parameters.OfType<CommandArgument>().OrderBy(argument => argument.Position))
            {
                argument.Position = index;
                index++;
            }

            return command;
        }

        private static IEnumerable<CommandParameter> GetParameters(CommandInfo command)
        {
            var result = new List<CommandParameter>();
            if (command.SettingsType != null)
            {
                var properties = command.SettingsType.GetTypeInfo().GetProperties();
                foreach (var property in properties)
                {
                    if (property.SetMethod == null)
                    {
                        continue;
                    }

                    // Create the parameter info.
                    var parameter = ParameterInfo.Create(command, property);

                    // Is this an option?
                    var option = property.GetCustomAttribute<OptionAttribute>();
                    if (option != null)
                    {
                        result.Add(CommandOption.Create(parameter, option));
                    }
                    else
                    {
                        // Is this an argument?
                        var argument = property.GetCustomAttribute<ArgumentAttribute>();
                        if (argument != null)
                        {
                            result.Add(new CommandArgument(parameter, argument.Position, argument.ArgumentName));
                        }
                    }
                }
            }
            return result;
        }

        private static Type GetSettingsType(Type commandType)
        {
            bool GetGenericTypeArguments(Type type, Type genericType, out Type[] genericTypeArguments)
            {
                while (type != null)
                {
                    var interfaceTypes = type.GetTypeInfo().GetInterfaces();
                    foreach (var @interface in interfaceTypes)
                    {
                        if (!@interface.GetTypeInfo().IsGenericType || @interface.GetGenericTypeDefinition() != genericType)
                        {
                            continue;
                        }
                        genericTypeArguments = @interface.GenericTypeArguments;
                        return true;
                    }
                    type = type.GetTypeInfo().BaseType;
                }
                genericTypeArguments = null;
                return false;
            }

            if (typeof(ICommand).GetTypeInfo().IsAssignableFrom(commandType))
            {
                if (GetGenericTypeArguments(commandType, typeof(ICommand<>), out Type[] genericTypeArguments))
                {
                    return genericTypeArguments[0];
                }
            }
            return null;
        }
    }
}
