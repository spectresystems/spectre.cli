using System;
using Shouldly;
using Spectre.Cli.Testing;
using Spectre.Cli.Testing.Data.Commands;
using Spectre.Cli.Testing.Data.Settings;
using Spectre.Cli.Testing.Fakes;
using Xunit;

namespace Spectre.Cli.Tests
{
    public sealed partial class CommandAppTests
    {
        public static class Parsing
        {
            public sealed class UnknownCommand
            {
                [Theory]
                [EmbeddedResourceData("Spectre.Cli.Tests/Properties/Resources/Exceptions/Parsing/UnknownCommand")]
                public void Should_Return_Correct_Text_When_Command_Is_Unknown(string expected)
                {
                    // Given
                    var fixture = new Fixture();
                    fixture.Configure(config =>
                    {
                        config.AddCommand<DogCommand>("dog");
                    });

                    // When
                    var result = fixture.Run("cat", "14");

                    // Then
                    result.ShouldBe(expected);
                }

                [Theory]
                [EmbeddedResourceData("Spectre.Cli.Tests/Properties/Resources/Exceptions/Parsing/UnknownRootCommand_Suggestion_ArgumentAfter")]
                public void Should_Return_Correct_Text_With_Suggestion_When_Root_Command_Followed_By_Argument_Is_Unknown_And_Distance_Is_Small(string expected)
                {
                    // Given
                    var fixture = new Fixture();
                    fixture.Configure(config =>
                    {
                        config.AddCommand<CatCommand>("cat");
                    });

                    // When
                    var result = fixture.Run("bat", "14");

                    // Then
                    result.ShouldBe(expected);
                }

                [Theory]
                [EmbeddedResourceData("Spectre.Cli.Tests/Properties/Resources/Exceptions/Parsing/UnknownCommand_Suggestion_ArgumentAfter")]
                public void Should_Return_Correct_Text_With_Suggestion_When_Command_Followed_By_Argument_Is_Unknown_And_Distance_Is_Small(string expected)
                {
                    // Given
                    var fixture = new Fixture();
                    fixture.Configure(config =>
                    {
                        config.AddBranch<CommandSettings>("dog", a =>
                        {
                            a.AddCommand<CatCommand>("cat");
                        });
                    });

                    // When
                    var result = fixture.Run("dog", "bat", "14");

                    // Then
                    result.ShouldBe(expected);
                }

                [Theory]
                [EmbeddedResourceData("Spectre.Cli.Tests/Properties/Resources/Exceptions/Parsing/UnknownRootCommand_Suggestion_NoArguments")]
                public void Should_Return_Correct_Text_With_Suggestion_And_No_Arguments_When_Root_Command_Is_Unknown_And_Distance_Is_Small(string expected)
                {
                    // Given
                    var fixture = new Fixture();
                    fixture.WithDefaultCommand<GenericCommand<EmptyCommandSettings>>();
                    fixture.Configure(config =>
                    {
                        config.AddCommand<GenericCommand<EmptyCommandSettings>>("cat");
                    });

                    // When
                    var result = fixture.Run("bat");

                    // Then
                    result.ShouldBe(expected);
                }

                [Theory]
                [EmbeddedResourceData("Spectre.Cli.Tests/Properties/Resources/Exceptions/Parsing/UnknownCommand_Suggestion_NoArguments")]
                public void Should_Return_Correct_Text_With_Suggestion_And_No_Arguments_When_Command_Is_Unknown_And_Distance_Is_Small(string expected)
                {
                    // Given
                    var fixture = new Fixture();
                    fixture.WithDefaultCommand<GenericCommand<EmptyCommandSettings>>();
                    fixture.Configure(configurator =>
                    {
                        configurator.AddBranch<CommandSettings>("dog", a =>
                        {
                            a.AddCommand<CatCommand>("cat");
                        });
                    });

                    // When
                    var result = fixture.Run("dog", "bat");

                    // Then
                    result.ShouldBe(expected);
                }

                [Theory]
                [EmbeddedResourceData("Spectre.Cli.Tests/Properties/Resources/Exceptions/Parsing/UnknownRootCommand_Suggestion_ArgumentBefore")]
                public void Should_Return_Correct_Text_With_Suggestion_When_Root_Command_After_Argument_Is_Unknown_And_Distance_Is_Small(string expected)
                {
                    // Given
                    var fixture = new Fixture();
                    fixture.WithDefaultCommand<GenericCommand<FooCommandSettings>>();
                    fixture.Configure(configurator =>
                    {
                        configurator.AddCommand<GenericCommand<BarCommandSettings>>("bar");
                    });

                    // When
                    var result = fixture.Run("qux", "bat");

                    // Then
                    result.ShouldBe(expected);
                }

                [Theory]
                [EmbeddedResourceData("Spectre.Cli.Tests/Properties/Resources/Exceptions/Parsing/UnknownCommand_Suggestion_ArgumentBefore")]
                public void Should_Return_Correct_Text_With_Suggestion_When_Command_After_Argument_Is_Unknown_And_Distance_Is_Small(string expected)
                {
                    // Given
                    var fixture = new Fixture();
                    fixture.Configure(configurator =>
                    {
                        configurator.AddBranch<FooCommandSettings>("foo", a =>
                        {
                            a.AddCommand<GenericCommand<BarCommandSettings>>("bar");
                        });
                    });

                    // When
                    var result = fixture.Run("foo", "qux", "bat");

                    // Then
                    result.ShouldBe(expected);
                }

                [Theory]
                [EmbeddedResourceData("Spectre.Cli.Tests/Properties/Resources/Exceptions/Parsing/UnknownCommand_NoArguments")]
                public void Should_Return_Correct_Text_For_Unknown_Command_When_Current_Command_Has_No_Arguments(string expected)
                {
                    // Given
                    var fixture = new Fixture();
                    fixture.Configure(configurator =>
                    {
                        configurator.AddCommand<EmptyCommand>("empty");
                    });

                    // When
                    var result = fixture.Run("empty", "other");

                    // Then
                    result.ShouldBe(expected);
                }
            }

            public sealed class CannotAssignValueToFlag
            {
                [Theory]
                [EmbeddedResourceData("Spectre.Cli.Tests/Properties/Resources/Exceptions/Parsing/CannotAssignValueToFlag_Long")]
                public void Should_Return_Correct_Text_For_Long_Option(string expected)
                {
                    // Given
                    var fixture = new Fixture();
                    fixture.Configure(configurator =>
                    {
                        configurator.AddCommand<DogCommand>("dog");
                    });

                    // When
                    var result = fixture.Run("dog", "--alive", "foo");

                    // Then
                    result.ShouldBe(expected);
                }

                [Theory]
                [EmbeddedResourceData("Spectre.Cli.Tests/Properties/Resources/Exceptions/Parsing/CannotAssignValueToFlag_Short")]
                public void Should_Return_Correct_Text_For_Short_Option(string expected)
                {
                    // Given
                    var fixture = new Fixture();
                    fixture.Configure(configurator =>
                    {
                        configurator.AddCommand<DogCommand>("dog");
                    });

                    // When
                    var result = fixture.Run("dog", "-a", "foo");

                    // Then
                    result.ShouldBe(expected);
                }
            }

            public sealed class NoValueForOption
            {
                [Theory]
                [EmbeddedResourceData("Spectre.Cli.Tests/Properties/Resources/Exceptions/Parsing/NoValueForOption_Long")]
                public void Should_Return_Correct_Text_For_Long_Option(string expected)
                {
                    // Given
                    var fixture = new Fixture();
                    fixture.Configure(configurator =>
                    {
                        configurator.AddCommand<DogCommand>("dog");
                    });

                    // When
                    var result = fixture.Run("dog", "--name");

                    // Then
                    result.ShouldBe(expected);
                }

                [Theory]
                [EmbeddedResourceData("Spectre.Cli.Tests/Properties/Resources/Exceptions/Parsing/NoValueForOption_Short")]
                public void Should_Return_Correct_Text_For_Short_Option(string expected)
                {
                    // Given
                    var fixture = new Fixture();
                    fixture.Configure(configurator =>
                    {
                        configurator.AddCommand<DogCommand>("dog");
                    });

                    // When
                    var result = fixture.Run("dog", "-n");

                    // Then
                    result.ShouldBe(expected);
                }
            }

            public sealed class NoMatchingArgument
            {
                [Theory]
                [EmbeddedResourceData("Spectre.Cli.Tests/Properties/Resources/Exceptions/Parsing/CouldNotMatchArgument")]
                public void Should_Return_Correct_Text(string expected)
                {
                    // Given
                    var fixture = new Fixture();
                    fixture.Configure(configurator =>
                    {
                        configurator.AddCommand<GiraffeCommand>("giraffe");
                    });

                    // When
                    var result = fixture.Run("giraffe", "foo", "bar", "baz");

                    // Then
                    result.ShouldBe(expected);
                }
            }

            public sealed class UnexpectedOption
            {
                [Theory]
                [EmbeddedResourceData("Spectre.Cli.Tests/Properties/Resources/Exceptions/Parsing/UnexpectedOption_Long")]
                public void Should_Return_Correct_Text_For_Long_Option(string expected)
                {
                    // Given
                    var fixture = new Fixture();
                    fixture.Configure(configurator =>
                    {
                        configurator.AddCommand<DogCommand>("dog");
                    });

                    // When
                    var result = fixture.Run("--foo");

                    // Then
                    result.ShouldBe(expected);
                }

                [Theory]
                [EmbeddedResourceData("Spectre.Cli.Tests/Properties/Resources/Exceptions/Parsing/UnexpectedOption_Short")]
                public void Should_Return_Correct_Text_For_Short_Option(string expected)
                {
                    // Given
                    var fixture = new Fixture();
                    fixture.Configure(configurator =>
                    {
                        configurator.AddCommand<DogCommand>("dog");
                    });

                    // When
                    var result = fixture.Run("-f");

                    // Then
                    result.ShouldBe(expected);
                }
            }

            public sealed class UnknownOption
            {
                [Theory]
                [EmbeddedResourceData("Spectre.Cli.Tests/Properties/Resources/Exceptions/Parsing/UnknownOption_Long")]
                public void Should_Return_Correct_Text_For_Long_Option_If_Strict_Mode_Is_Enabled(string expected)
                {
                    // Given
                    var fixture = new Fixture();
                    fixture.Configure(configurator =>
                    {
                        configurator.UseStrictParsing();
                        configurator.AddCommand<DogCommand>("dog");
                    });

                    // When
                    var result = fixture.Run("dog", "--unknown");

                    // Then
                    result.ShouldBe(expected);
                }

                [Theory]
                [EmbeddedResourceData("Spectre.Cli.Tests/Properties/Resources/Exceptions/Parsing/UnknownOption_Short")]
                public void Should_Return_Correct_Text_For_Short_Option_If_Strict_Mode_Is_Enabled(string expected)
                {
                    // Given
                    var fixture = new Fixture();
                    fixture.Configure(configurator =>
                    {
                        configurator.UseStrictParsing();
                        configurator.AddCommand<DogCommand>("dog");
                    });

                    // When
                    var result = fixture.Run("dog", "-u");

                    // Then
                    result.ShouldBe(expected);
                }
            }

            public sealed class UnterminatedQuote
            {
                [Theory]
                [EmbeddedResourceData("Spectre.Cli.Tests/Properties/Resources/Exceptions/Parsing/Tokenization/UnterminatedQuote")]
                public void Should_Return_Correct_Text(string expected)
                {
                    // Given
                    var fixture = new Fixture();
                    fixture.Configure(configurator =>
                    {
                        configurator.AddCommand<DogCommand>("dog");
                    });

                    // When
                    var result = fixture.Run("--name", "\"Rufus");

                    // Then
                    result.ShouldBe(expected);
                }
            }

            public sealed class OptionWithoutName
            {
                [Theory]
                [EmbeddedResourceData("Spectre.Cli.Tests/Properties/Resources/Exceptions/Parsing/Tokenization/OptionHasNoName_Short")]
                public void Should_Return_Correct_Text_For_Short_Option(string expected)
                {
                    // Given
                    var fixture = new Fixture();
                    fixture.Configure(configurator =>
                    {
                        configurator.AddCommand<DogCommand>("dog");
                    });

                    // When
                    var result = fixture.Run("dog", "-", " ");

                    // Then
                    result.ShouldBe(expected);
                }

                [Theory]
                [EmbeddedResourceData("Spectre.Cli.Tests/Properties/Resources/Exceptions/Parsing/Tokenization/OptionValueWasExpected_Equality_Long", '=')]
                [EmbeddedResourceData("Spectre.Cli.Tests/Properties/Resources/Exceptions/Parsing/Tokenization/OptionValueWasExpected_Colon_Long", ':')]
                public void Should_Return_Correct_Text_For_Expected_Long_Option_Value(string expected, char separator)
                {
                    // Given
                    var fixture = new Fixture();
                    fixture.Configure(configurator =>
                    {
                        configurator.AddCommand<DogCommand>("dog");
                    });

                    // When
                    var result = fixture.Run("dog", $"--foo{separator}");

                    // Then
                    result.ShouldBe(expected);
                }

                [Theory]
                [EmbeddedResourceData("Spectre.Cli.Tests/Properties/Resources/Exceptions/Parsing/Tokenization/OptionValueWasExpected_Equality_Short", '=')]
                [EmbeddedResourceData("Spectre.Cli.Tests/Properties/Resources/Exceptions/Parsing/Tokenization/OptionValueWasExpected_Colon_Short", ':')]
                public void Should_Return_Correct_Text_For_Expected_Short_Option_Value(string expected, char separator)
                {
                    // Given
                    var fixture = new Fixture();
                    fixture.Configure(configurator =>
                    {
                        configurator.AddCommand<DogCommand>("dog");
                    });

                    // When
                    var result = fixture.Run("dog", $"-f{separator}");

                    // Then
                    result.ShouldBe(expected);
                }
            }

            public sealed class InvalidShortOptionName
            {
                [Theory]
                [EmbeddedResourceData("Spectre.Cli.Tests/Properties/Resources/Exceptions/Parsing/Tokenization/InvalidShortOptionName")]
                public void Should_Return_Correct_Text(string expected)
                {
                    // Given
                    var fixture = new Fixture();
                    fixture.Configure(configurator =>
                    {
                        configurator.AddCommand<DogCommand>("dog");
                    });

                    // When
                    var result = fixture.Run("dog", $"-f0o");

                    // Then
                    result.ShouldBe(expected);
                }
            }

            public sealed class LongOptionNameIsOneCharacter
            {
                [Theory]
                [EmbeddedResourceData("Spectre.Cli.Tests/Properties/Resources/Exceptions/Parsing/Tokenization/LongOptionNameIsOneCharacter")]
                public void Should_Return_Correct_Text(string expected)
                {
                    // Given
                    var fixture = new Fixture();
                    fixture.Configure(configurator =>
                    {
                        configurator.AddCommand<DogCommand>("dog");
                    });

                    // When
                    var result = fixture.Run("dog", $"--f");

                    // Then
                    result.ShouldBe(expected);
                }
            }

            public sealed class LongOptionNameIsMissing
            {
                [Theory]
                [EmbeddedResourceData("Spectre.Cli.Tests/Properties/Resources/Exceptions/Parsing/Tokenization/LongOptionNameIsMissing")]
                public void Should_Return_Correct_Text(string expected)
                {
                    // Given
                    var fixture = new Fixture();
                    fixture.Configure(configurator =>
                    {
                        configurator.AddCommand<DogCommand>("dog");
                    });

                    // When
                    var result = fixture.Run("dog", $"-- ");

                    // Then
                    result.ShouldBe(expected);
                }
            }

            public sealed class LongOptionNameStartWithDigit
            {
                [Theory]
                [EmbeddedResourceData("Spectre.Cli.Tests/Properties/Resources/Exceptions/Parsing/Tokenization/LongOptionNameStartWithDigit")]
                public void Should_Return_Correct_Text(string expected)
                {
                    // Given
                    var fixture = new Fixture();
                    fixture.Configure(configurator =>
                    {
                        configurator.AddCommand<DogCommand>("dog");
                    });

                    // When
                    var result = fixture.Run("dog", $"--1foo");

                    // Then
                    result.ShouldBe(expected);
                }
            }

            public sealed class LongOptionNameContainSymbol
            {
                [Theory]
                [EmbeddedResourceData("Spectre.Cli.Tests/Properties/Resources/Exceptions/Parsing/Tokenization/LongOptionNameContainSymbol")]
                public void Should_Return_Correct_Text(string expected)
                {
                    // Given
                    var fixture = new Fixture();
                    fixture.Configure(configurator =>
                    {
                        configurator.AddCommand<DogCommand>("dog");
                    });

                    // When
                    var result = fixture.Run("dog", $"--f€oo");

                    // Then
                    result.ShouldBe(expected);
                }

                [Theory]
                [InlineData("--f-oo")]
                [InlineData("--f-o-o")]
                [InlineData("--f_oo")]
                [InlineData("--f_o_o")]
                public void Should_Allow_Special_Symbols_In_Name(string option)
                {
                    // Given
                    var fixture = new Fixture();
                    fixture.Configure(configurator =>
                    {
                        configurator.AddCommand<DogCommand>("dog");
                    });

                    // When
                    var result = fixture.Run("dog", option);

                    // Then
                    result.ShouldBe("Error: Command 'dog' is missing required argument 'AGE'.");
                }
            }
        }

        internal sealed class Fixture
        {
            private Action<CommandApp> _appConfiguration = _ => { };
            private Action<IConfigurator> _configuration;

            public void WithDefaultCommand<T>()
                where T : class, ICommand
            {
                _appConfiguration = (app) => app.SetDefaultCommand<T>();
            }

            public void Configure(Action<IConfigurator> action)
            {
                _configuration = action;
            }

            public string Run(params string[] args)
            {
                var writer = new FakeConsoleWriter();

                var app = new CommandApp();
                _appConfiguration?.Invoke(app);

                app.Configure(_configuration);
                app.Configure(c => c.SetOut(writer));
                app.Run(args);

                return writer.ToString();
            }
        }
    }
}
