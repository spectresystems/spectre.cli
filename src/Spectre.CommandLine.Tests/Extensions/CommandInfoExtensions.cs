using System;
using System.Linq;
using Spectre.CommandLine.Internal.Modelling;

// ReSharper disable once CheckNamespace
namespace Spectre.CommandLine.Tests
{
    internal static class CommandInfoExtensions
    {
        public static CommandOption GetOption(this CommandInfo command, Func<CommandOption, bool> predicate)
        {
            return command.Parameters.OfType<CommandOption>().SingleOrDefault(predicate);
        }
    }
}
