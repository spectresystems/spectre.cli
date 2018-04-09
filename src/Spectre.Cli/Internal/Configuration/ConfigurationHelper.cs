using System;
using System.Reflection;

namespace Spectre.Cli.Internal.Configuration
{
    internal static class ConfigurationHelper
    {
        public static Type GetSettingsType(Type commandType)
        {
            bool GetGenericTypeArguments(Type type, Type genericType, out Type[] genericTypeArguments)
            {
                while (type != null)
                {
                    foreach (var @interface in type.GetTypeInfo().GetInterfaces())
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

            if (typeof(ICommand).GetTypeInfo().IsAssignableFrom(commandType) &&
                GetGenericTypeArguments(commandType, typeof(ICommand<>), out var result))
            {
                return result[0];
            }

            return null;
        }
    }
}
