using System;
using Shouldly;
using Spectre.Cli.Exceptions;
using Spectre.Cli.Testing;
using Spectre.Cli.Testing.Data.Commands;
using Spectre.Cli.Testing.Fakes;
using Xunit;

namespace Spectre.Cli.Tests.Annotations
{
    public sealed partial class CommandOptionAttributeTests
    {
        public sealed class TheUnexpectedCharacterMethod
        {
            public sealed class Settings : CommandSettings
            {
                [CommandOption("<FOO> $ <BAR>")]
                public string Foo { get; set; }
            }

            [Theory]
            [EmbeddedResourceData("Spectre.Cli.Tests/Resources/Exceptions/Template/UnexpectedCharacter")]
            public void Should_Return_Correct_Text(string expected)
            {
                // Given, When
                var (message, result) = Fixture.Run<Settings>();

                // Then
                message.ShouldBe("Encountered unexpected character '$'.");
                result.ShouldBe(expected);
            }
        }

        public sealed class TheUnterminatedValueNameMethod
        {
            public sealed class Settings : CommandSettings
            {
                [CommandOption("--foo|-f <BAR")]
                public string Foo { get; set; }
            }

            [Theory]
            [EmbeddedResourceData("Spectre.Cli.Tests/Resources/Exceptions/Template/UnterminatedValueName")]
            public void Should_Return_Correct_Text(string expected)
            {
                // Given, When
                var (message, result) = Fixture.Run<Settings>();

                // Then
                message.ShouldBe("Encountered unterminated value name 'BAR'.");
                result.ShouldBe(expected);
            }
        }

        public sealed class TheOptionsMustHaveNameMethod
        {
            public sealed class Settings : CommandSettings
            {
                [CommandOption("--foo|-")]
                public string Foo { get; set; }
            }

            [Theory]
            [EmbeddedResourceData("Spectre.Cli.Tests/Resources/Exceptions/Template/OptionsMustHaveName")]
            public void Should_Return_Correct_Text(string expected)
            {
                // Given, When
                var (message, result) = Fixture.Run<Settings>();

                // Then
                message.ShouldBe("Options without name are not allowed.");
                result.ShouldBe(expected);
            }
        }

        public sealed class TheOptionNamesCannotStartWithDigitMethod
        {
            public sealed class Settings : CommandSettings
            {
                [CommandOption("--1foo")]
                public string Foo { get; set; }
            }

            [Theory]
            [EmbeddedResourceData("Spectre.Cli.Tests/Resources/Exceptions/Template/OptionNamesCannotStartWithDigit")]
            public void Should_Return_Correct_Text(string expected)
            {
                // Given, When
                var (message, result) = Fixture.Run<Settings>();

                // Then
                message.ShouldBe("Option names cannot start with a digit.");
                result.ShouldBe(expected);
            }
        }

        public sealed class TheInvalidCharacterInOptionNameMethod
        {
            public sealed class Settings : CommandSettings
            {
                [CommandOption("--f$oo")]
                public string Foo { get; set; }
            }

            [Theory]
            [EmbeddedResourceData("Spectre.Cli.Tests/Resources/Exceptions/Template/InvalidCharacterInOptionName")]
            public void Should_Return_Correct_Text(string expected)
            {
                // Given, When
                var (message, result) = Fixture.Run<Settings>();

                // Then
                message.ShouldBe("Encountered invalid character '$' in option name.");
                result.ShouldBe(expected);
            }
        }

        public sealed class TheLongOptionMustHaveMoreThanOneCharacterMethod
        {
            public sealed class Settings : CommandSettings
            {
                [CommandOption("--f")]
                public string Foo { get; set; }
            }

            [Theory]
            [EmbeddedResourceData("Spectre.Cli.Tests/Resources/Exceptions/Template/LongOptionMustHaveMoreThanOneCharacter")]
            public void Should_Return_Correct_Text(string expected)
            {
                // Given, When
                var (message, result) = Fixture.Run<Settings>();

                // Then
                message.ShouldBe("Long option names must consist of more than one character.");
                result.ShouldBe(expected);
            }
        }

        public sealed class TheShortOptionMustOnlyBeOneCharacterMethod
        {
            public sealed class Settings : CommandSettings
            {
                [CommandOption("--foo|-bar")]
                public string Foo { get; set; }
            }

            [Theory]
            [EmbeddedResourceData("Spectre.Cli.Tests/Resources/Exceptions/Template/ShortOptionMustOnlyBeOneCharacter")]
            public void Should_Return_Correct_Text(string expected)
            {
                // Given, When
                var (message, result) = Fixture.Run<Settings>();

                // Then
                message.ShouldBe("Short option names can not be longer than one character.");
                result.ShouldBe(expected);
            }
        }

        public sealed class TheMultipleOptionValuesAreNotSupportedMethod
        {
            public sealed class Settings : CommandSettings
            {
                [CommandOption("-f|--foo <FOO> <BAR>")]
                public string Foo { get; set; }
            }

            [Theory]
            [EmbeddedResourceData("Spectre.Cli.Tests/Resources/Exceptions/Template/MultipleOptionValuesAreNotSupported")]
            public void Should_Return_Correct_Text(string expected)
            {
                // Given, When
                var (message, result) = Fixture.Run<Settings>();

                // Then
                message.ShouldBe("Multiple option values are not supported.");
                result.ShouldBe(expected);
            }
        }

        public sealed class TheInvalidCharacterInValueNameMethod
        {
            public sealed class Settings : CommandSettings
            {
                [CommandOption("-f|--foo <F$OO>")]
                public string Foo { get; set; }
            }

            [Theory]
            [EmbeddedResourceData("Spectre.Cli.Tests/Resources/Exceptions/Template/InvalidCharacterInValueName")]
            public void Should_Return_Correct_Text(string expected)
            {
                // Given, When
                var (message, result) = Fixture.Run<Settings>();

                // Then
                message.ShouldBe("Encountered invalid character '$' in value name.");
                result.ShouldBe(expected);
            }
        }

        public sealed class TheMissingLongAndShortNameMethod
        {
            public sealed class Settings : CommandSettings
            {
                [CommandOption("<FOO>")]
                public string Foo { get; set; }
            }

            [Theory]
            [EmbeddedResourceData("Spectre.Cli.Tests/Resources/Exceptions/Template/MissingLongAndShortName")]
            public void Should_Return_Correct_Text(string expected)
            {
                // Given, When
                var (message, result) = Fixture.Run<Settings>();

                // Then
                message.ShouldBe("No long or short name for option has been specified.");
                result.ShouldBe(expected);
            }
        }

        private static class Fixture
        {
            public static (string Message, string Output) Run<TSettings>(params string[] args)
                where TSettings : CommandSettings
            {
                var app = new CommandApp();
                app.Configure(c => c.PropagateExceptions());
                app.Configure(c => c.AddCommand<GenericCommand<TSettings>>("foo"));

                try
                {
                    app.Run(args);
                }
                catch (TemplateException ex)
                {
                    using (var writer = new FakeConsole())
                    {
                        ex.Render(writer);

                        return (ex.Message, writer.Output
                            .NormalizeLineEndings()
                            .TrimLines()
                            .Trim());
                    }
                }

                throw new InvalidOperationException("Expected a template exception");
            }
        }
    }
}
