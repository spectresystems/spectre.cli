using Shouldly;
using Spectre.Cli.Exceptions;
using Spectre.Cli.Testing;
using Spectre.Cli.Testing.Data.Commands;
using Spectre.Cli.Testing.Data.Settings;
using Spectre.Cli.Testing.Fakes;
using Xunit;

namespace Spectre.Cli.Tests
{
    public sealed partial class CommandAppTests
    {
        public sealed class Vectors
        {
            [Fact]
            public void Should_Throw_If_A_Single_Command_Has_Multiple_Argument_Vectors()
            {
                // Given
                var app = new CommandApp();
                app.Configure(config =>
                {
                    config.PropagateExceptions();
                    config.AddCommand<GenericCommand<MultipleArgumentVectorSettings>>("multi");
                });

                // When
                var result = Record.Exception(() => app.Run(new[] { "multi", "a", "b", "c" }));

                // Then
                result.ShouldBeOfType<ConfigurationException>().And(ex =>
                {
                    ex.Message.ShouldBe("The command 'multi' specifies more than one vector argument.");
                });
            }

            [Fact]
            public void Should_Throw_If_An_Argument_Vector_Is_Not_Specified_Last()
            {
                // Given
                var app = new CommandApp();
                app.Configure(config =>
                {
                    config.PropagateExceptions();
                    config.AddCommand<GenericCommand<MultipleArgumentVectorSpecifiedFirstSettings>>("multi");
                });

                // When
                var result = Record.Exception(() => app.Run(new[] { "multi", "a", "b", "c" }));

                // Then
                result.ShouldBeOfType<ConfigurationException>().And(ex =>
                {
                    ex.Message.ShouldBe("The command 'multi' specifies an argument vector that is not the last argument.");
                });
            }

            [Fact]
            public void Should_Assign_Values_To_Argument_Vector()
            {
                // Given
                var resolver = new FakeTypeResolver();
                var settings = new ArgumentVectorSettings();
                resolver.Register(settings);

                var app = new CommandApp(new FakeTypeRegistrar(resolver));
                app.Configure(config =>
                {
                    config.PropagateExceptions();
                    config.AddCommand<GenericCommand<ArgumentVectorSettings>>("multi");
                });

                // When
                var result = app.Run(new[]
                {
                    "multi", "a", "b", "c",
                });

                // Then
                result.ShouldBe(0);
                settings.Foo.Length.ShouldBe(3);
                settings.Foo[0].ShouldBe("a");
                settings.Foo[1].ShouldBe("b");
                settings.Foo[2].ShouldBe("c");
            }

            [Fact]
            public void Should_Assign_Values_To_Option_Vector()
            {
                // Given
                var resolver = new FakeTypeResolver();
                var settings = new OptionVectorSettings();
                resolver.Register(settings);

                var app = new CommandApp(new FakeTypeRegistrar(resolver));
                app.Configure(config =>
                {
                    config.PropagateExceptions();
                    config.AddCommand<OptionVectorCommand>("cmd");
                });

                // When
                var result = app.Run(new[]
                {
                    "cmd", "--foo", "red",
                    "--bar", "4", "--foo", "blue",
                });

                // Then
                result.ShouldBe(0);
                settings.Foo.ShouldBe(new string[] { "red", "blue" });
                settings.Bar.ShouldBe(new int[] { 4 });
            }
        }
    }
}
