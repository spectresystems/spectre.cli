using Shouldly;
using Spectre.Cli.Internal.Configuration;

// ReSharper disable once CheckNamespace
namespace Spectre.Cli.Tests
{
    internal static class ConfiguredCommandExtensions
    {
        public static void ShouldBeCommand<TCommand, TSettings>(this ConfiguredCommand command)
        {
            command.CommandType.ShouldBe<TCommand>();
            command.SettingsType.ShouldBe<TSettings>();
        }

        public static void ShouldBeProxy<TSettings>(this ConfiguredCommand command)
        {
            command.CommandType.ShouldBeNull();
            command.SettingsType.ShouldBe<TSettings>();
        }
    }
}