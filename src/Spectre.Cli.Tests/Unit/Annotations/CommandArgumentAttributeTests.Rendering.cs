using Shouldly;
using Spectre.Cli.Testing;
using Spectre.Cli.Testing.Data.Commands;
using Spectre.Cli.Testing.Fakes;
using Xunit;

namespace Spectre.Cli.Tests.Annotations
{
    public sealed partial class CommandArgumentAttributeTests
    {
        public sealed class TheArgumentCannotContainOptionsMethod
        {
            public sealed class Settings : CommandSettings
            {
                [CommandArgument(0, "--foo <BAR>")]
                public string Foo { get; set; }
            }

            [Theory]
            [EmbeddedResourceData("Spectre.Cli.Tests/Resources/Exceptions/Template/ArgumentCannotContainOptions")]
            public void Should_Return_Correct_Text(string expected)
            {
                // Given, When
                var result = Fixture.Run<Settings>();

                // Then
                result.ShouldBe(expected);
            }
        }

        public sealed class TheMultipleValuesAreNotSupportedMethod
        {
            public sealed class Settings : CommandSettings
            {
                [CommandArgument(0, "<FOO> <BAR>")]
                public string Foo { get; set; }
            }

            [Theory]
            [EmbeddedResourceData("Spectre.Cli.Tests/Resources/Exceptions/Template/MultipleValuesAreNotSupported")]
            public void Should_Return_Correct_Text(string expected)
            {
                // Given, When
                var result = Fixture.Run<Settings>();

                // Then
                result.ShouldBe(expected);
            }
        }

        public sealed class TheValuesMustHaveNameMethod
        {
            public sealed class Settings : CommandSettings
            {
                [CommandArgument(0, "<>")]
                public string Foo { get; set; }
            }

            [Theory]
            [EmbeddedResourceData("Spectre.Cli.Tests/Resources/Exceptions/Template/ValuesMustHaveName")]
            public void Should_Return_Correct_Text(string expected)
            {
                // Given, When
                var result = Fixture.Run<Settings>();

                // Then
                result.ShouldBe(expected);
            }
        }

        private static class Fixture
        {
            public static string Run<TSettings>(params string[] args)
                where TSettings : CommandSettings
            {
                using (var writer = new FakeConsole())
                {
                    var app = new CommandApp();
                    app.Configure(c => c.ConfigureConsole(writer));
                    app.Configure(c => c.AddCommand<GenericCommand<TSettings>>("foo"));
                    app.Run(args);

                    return writer.Output
                        .NormalizeLineEndings()
                        .TrimLines()
                        .Trim();
                }
            }
        }
    }
}
