using System;
using System.Linq;
using Spectre.Cli.Internal.Modelling;

// ReSharper disable once CheckNamespace
namespace Spectre.Cli.Tests
{
    internal static class CommandInfoExtensions
    {
        public static CommandOption GetOption(this CommandInfo command, Func<CommandOption, bool> predicate)
        {
            return command.Parameters.OfType<CommandOption>().SingleOrDefault(predicate);
        }
    }
}
