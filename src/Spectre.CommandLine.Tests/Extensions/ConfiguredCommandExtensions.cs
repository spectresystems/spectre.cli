using Shouldly;
using Spectre.CommandLine.Internal.Configuration;

// ReSharper disable once CheckNamespace
namespace Spectre.CommandLine.Tests
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