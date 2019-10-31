using Shouldly;
using Spectre.Cli.Exceptions;
using Spectre.Cli.Testing;
using Spectre.Cli.Testing.Data.Commands;
using Spectre.Cli.Testing.Fakes;
using Xunit;

using DefaultValue = System.ComponentModel.DefaultValueAttribute;

namespace Spectre.Cli.Tests
{
    public sealed partial class CommandAppTests
    {
        public sealed class FlagValues
        {
            private sealed class Settings : CommandSettings
            {
                [CommandOption("--serve [PORT]")]
                public FlagValue<int> Serve { get; set; }
            }

            private sealed class SettingsWithNullableValueType : CommandSettings
            {
                [CommandOption("--serve [PORT]")]
                public FlagValue<int?> Serve { get; set; }
            }

            private sealed class SettingsWithOptionalOptionButNoFlagValue : CommandSettings
            {
                [CommandOption("--serve [PORT]")]
                public int Serve { get; set; }
            }

            private sealed class SettingsWithDefaultValue : CommandSettings
            {
                [CommandOption("--serve [PORT]")]
                [DefaultValue(987)]
                public FlagValue<int> Serve { get; set; }
            }

            [Fact]
            public void Should_Throw_If_Command_Option_Value_Is_Optional_But_Type_Is_Not_A_Flag_Value()
            {
                // Given
                var resolver = new FakeTypeResolver();
                var settings = new SettingsWithOptionalOptionButNoFlagValue();
                resolver.Register(settings);

                var app = new CommandApp(new FakeTypeRegistrar(resolver));
                app.Configure(config =>
                {
                    config.PropagateExceptions();
                    config.AddCommand<GenericCommand<SettingsWithOptionalOptionButNoFlagValue>>("foo");
                });

                // When
                var result = Record.Exception(() => app.Run(new[] { "foo", "--serve", "123" }));

                // Then
                result.ShouldBeOfType<ConfigurationException>().And(ex =>
                {
                    ex.Message.ShouldBe("The option 'serve' has an optional value but does not implement IOptionalValue.");
                });
            }

            [Fact]
            public void Should_Set_Flag_And_Value_If_Both_Were_Provided()
            {
                // Given
                var resolver = new FakeTypeResolver();
                var settings = new Settings();
                resolver.Register(settings);

                var app = new CommandApp(new FakeTypeRegistrar(resolver));
                app.Configure(config =>
                {
                    config.PropagateExceptions();
                    config.AddCommand<GenericCommand<Settings>>("foo");
                });

                // When
                var result = app.Run(new[]
                {
                    "foo", "--serve", "123"
                });

                // Then
                result.ShouldBe(0);
                settings.Serve.IsSet.ShouldBeTrue();
                settings.Serve.Value.ShouldBe(123);
            }

            [Fact]
            public void Should_Only_Set_Flag_If_No_Value_Was_Provided()
            {
                // Given
                var resolver = new FakeTypeResolver();
                var settings = new Settings();
                resolver.Register(settings);

                var app = new CommandApp(new FakeTypeRegistrar(resolver));
                app.Configure(config =>
                {
                    config.PropagateExceptions();
                    config.AddCommand<GenericCommand<Settings>>("foo");
                });

                // When
                var result = app.Run(new[]
                {
                    "foo", "--serve"
                });

                // Then
                result.ShouldBe(0);
                settings.Serve.IsSet.ShouldBeTrue();
                settings.Serve.Value.ShouldBe(0);
            }

            [Fact]
            public void Should_Set_Value_To_Default_Value_If_None_Was_Explicitly_Set()
            {
                // Given
                var resolver = new FakeTypeResolver();
                var settings = new SettingsWithDefaultValue();
                resolver.Register(settings);

                var app = new CommandApp(new FakeTypeRegistrar(resolver));
                app.Configure(config =>
                {
                    config.PropagateExceptions();
                    config.AddCommand<GenericCommand<SettingsWithDefaultValue>>("foo");
                });

                // When
                var result = app.Run(new[]
                {
                    "foo", "--serve"
                });

                // Then
                result.ShouldBe(0);
                settings.Serve.IsSet.ShouldBeTrue();
                settings.Serve.Value.ShouldBe(987);
            }

            [Fact]
            public void Should_Create_Unset_Instance_If_Flag_Was_Not_Set()
            {
                // Given
                var resolver = new FakeTypeResolver();
                var settings = new Settings();
                resolver.Register(settings);

                var app = new CommandApp(new FakeTypeRegistrar(resolver));
                app.Configure(config =>
                {
                    config.PropagateExceptions();
                    config.AddCommand<GenericCommand<Settings>>("foo");
                });

                // When
                var result = app.Run(new[]
                {
                    "foo"
                });

                // Then
                result.ShouldBe(0);
                settings.Serve.IsSet.ShouldBeFalse();
                settings.Serve.Value.ShouldBe(0);
            }

            [Fact]
            public void Should_Create_Unset_Instance_With_Null_For_Nullable_Value_Type_If_Flag_Was_Not_Set()
            {
                // Given
                var resolver = new FakeTypeResolver();
                var settings = new SettingsWithNullableValueType();
                resolver.Register(settings);

                var app = new CommandApp(new FakeTypeRegistrar(resolver));
                app.Configure(config =>
                {
                    config.PropagateExceptions();
                    config.AddCommand<GenericCommand<SettingsWithNullableValueType>>("foo");
                });

                // When
                var result = app.Run(new[]
                {
                    "foo"
                });

                // Then
                result.ShouldBe(0);
                settings.Serve.IsSet.ShouldBeFalse();
                settings.Serve.Value.ShouldBeNull();
            }
        }
    }
}
