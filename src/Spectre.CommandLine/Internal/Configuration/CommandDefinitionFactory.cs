using System;
using System.Reflection;

// ReSharper disable once CheckNamespace
namespace Spectre.CommandLine.Internal
{
    internal static class CommandDefinitionFactory
    {
        public static CommandDefinition CreateProxy(IResolver resolver, CommandDefinition parent, string name, Type settingsType)
        {
            var command = new CommandDefinition(parent, name, null, settingsType);

            var options = OptionDefinitionFactory.CreateOptions(resolver, command, settingsType);
            foreach (var option in options)
            {
                command.Options.Add(option);
            }

            return command;
        }

        public static CommandDefinition CreateCommand(IResolver resolver, CommandDefinition parent, string name, Type commandType)
        {
            var settingsType = GetSettingsType(commandType);
            var command = new CommandDefinition(parent, name, commandType, settingsType);

            var options = OptionDefinitionFactory.CreateOptions(resolver, command, settingsType);
            foreach (var option in options)
            {
                command.Options.Add(option);
            }

            return command;
        }

        private static Type GetSettingsType(Type command)
        {
            if (GetGenericTypeArguments(command, typeof(ICommand<>), out Type[] genericTypeArguments))
            {
                return genericTypeArguments[0];
            }
            throw new InvalidOperationException($"Could not determine settings type for command of type '{command.FullName}'");
        }

        private static bool GetGenericTypeArguments(Type type, Type genericType, out Type[] genericTypeArguments)
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
    }
}
