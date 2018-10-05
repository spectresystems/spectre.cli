using System;

namespace Spectre.Cli.Internal
{
    internal static class TypeRegistrarExtensions
    {
        public static void RegisterCommand(this ITypeRegistrar registrar, Type commandType, Type settingsType)
        {
            registrar?.Register(typeof(ICommand), commandType);
            registrar?.Register(commandType, commandType);
            registrar?.Register(settingsType, settingsType);
        }
    }
}
