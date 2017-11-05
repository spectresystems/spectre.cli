using System;
using System.Linq;
using Shouldly;
using Spectre.CommandLine.Internal.Modelling;

// ReSharper disable once CheckNamespace
namespace Spectre.CommandLine.Tests
{
    internal static class CommandInfoExtensions
    {
        public static void ShouldBeCommand<TCommand, TSettings>(this CommandInfo command)
        {
            command.IsProxy.ShouldBe(false);
            command.CommandType.ShouldBe<TCommand>();
            command.SettingsType.ShouldBe<TSettings>();
        }

        public static void ShouldBeProxy<TSettings>(this CommandInfo command)
        {
            command.IsProxy.ShouldBe(true, $"The command '{command.Name}' is not a proxy.");
            command.CommandType.ShouldBeNull();
            command.SettingsType.ShouldBe<TSettings>();
        }

        public static CommandOption GetOption(this CommandInfo command, Func<CommandOption, bool> predicate)
        {
            return command.Parameters.OfType<CommandOption>().SingleOrDefault(predicate);
        }
    }
}
