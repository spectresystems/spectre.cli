using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using Spectre.CommandLine.Annotations;

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

            var command = new CommandInfo(parent, name, commandType, settingsType);
            command.Description = commandType.GetTypeInfo().GetCustomAttribute<DescriptionAttribute>()?.Description;
            command.Parameters.AddRange(GetParameters(command));
            return command;
        }

        private static IEnumerable<CommandParameter> GetParameters(CommandInfo command)
        {
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
                    var parameter = CreateParameterInfo(command, property);

                    // Is this an option?
                    var option = property.GetCustomAttribute<OptionAttribute>();
                    if (option != null)
                    {
                        yield return CreateOptionDefinition(option, parameter);
                    }
                }
            }
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

        private static ParameterInfo CreateParameterInfo(CommandInfo command, PropertyInfo property)
        {
            var description = property.GetCustomAttribute<DescriptionAttribute>()?.Description;
            var converter = property.GetCustomAttribute<TypeConverterAttribute>();
            var required = property.GetCustomAttribute<RequiredAttribute>() != null;
            var inherited = property.DeclaringType != command.SettingsType;

            var type = property.PropertyType == typeof(bool)
                ? ParameterType.Flag : ParameterType.Single;

            return new ParameterInfo(property.PropertyType, type, property, description, inherited, converter, required);
        }

        private static CommandOption CreateOptionDefinition(OptionAttribute attribute, ParameterInfo parameter)
        {
            return new CommandOption(
                parameter,
                attribute,
                parameter.Property.GetCustomAttribute<DefaultValueAttribute>());
        }
    }
}
