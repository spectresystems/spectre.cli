using System;
using Spectre.Cli.Internal.Configuration;

namespace Spectre.Cli.Internal
{
    internal static class TypeRegistrarExtensions
    {
        public static void RegisterCommand(this ITypeRegistrar registrar, ConfiguredCommand command)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            registrar?.Register(typeof(ICommand), command.CommandType);
            registrar?.Register(command.CommandType, command.CommandType);
            registrar?.Register(command.SettingsType, command.SettingsType);
        }
    }
}
