using System;
using System.Collections.Generic;
using System.Text;
using Shouldly;
using Spectre.Cli.Internal.Configuration;
using Spectre.Cli.Internal.Exceptions;
using Spectre.Cli.Internal.Modelling;
using Spectre.Cli.Internal.Parsing;
using Spectre.Cli.Internal.Rendering;
using Spectre.Cli.Tests.Data;
using Spectre.Cli.Tests.Data.Settings;
using Spectre.Cli.Tests.Fakes;
using Spectre.Cli.Tests.Utilities;
using Xunit;

namespace Spectre.Cli.Tests.Unit.Internal.Exceptions
{
    public static class ParseExceptionTests
    {
        public sealed class UnknownCommand
        {
            [Theory]
            [EmbeddedResourceData("Spectre.Cli.Tests/Data/Resources/Exceptions/Parsing/UnknownCommand")]
            public void Should_Return_Correct_Text_When_Command_Is_Unknown(string expected)
            {
                // Given
                var configurator = new Configurator(new FakeTypeRegistrar());
                configurator.AddCommand<DogCommand>("dog");

                // When
                var result = Fixture.GetParseMessage(new[] { "cat", "14" }, configurator);

                // Then
                result.ShouldBe(expected);
            }

            [Theory]
            [EmbeddedResourceData("Spectre.Cli.Tests/Data/Resources/Exceptions/Parsing/UnknownRootCommand_Suggestion_ArgumentAfter")]
            public void Should_Return_Correct_Text_With_Suggestion_When_Root_Command_Followed_By_Argument_Is_Unknown_And_Distance_Is_Small(string expected)
            {
                // Given
                var configurator = new Configurator(new FakeTypeRegistrar(), typeof(GenericCommand<EmptySettings>));
                configurator.AddCommand<CatCommand>("cat");

                // When
                var result = Fixture.GetParseMessage(new[] { "bat", "14" }, configurator);

                // Then
                result.ShouldBe(expected);
            }

            [Theory]
            [EmbeddedResourceData("Spectre.Cli.Tests/Data/Resources/Exceptions/Parsing/UnknownCommand_Suggestion_ArgumentAfter")]
            public void Should_Return_Correct_Text_With_Suggestion_When_Command_Followed_By_Argument_Is_Unknown_And_Distance_Is_Small(string expected)
            {
                // Given
                var configurator = new Configurator(new FakeTypeRegistrar());
                configurator.AddBranch<CommandSettings>("dog", a =>
                {
                    a.AddCommand<CatCommand>("cat");
                });

                // When
                var result = Fixture.GetParseMessage(new[] { "dog", "bat", "14" }, configurator);

                // Then
                result.ShouldBe(expected);
            }

            [Theory]
            [EmbeddedResourceData("Spectre.Cli.Tests/Data/Resources/Exceptions/Parsing/UnknownRootCommand_Suggestion_NoArguments")]
            public void Should_Return_Correct_Text_With_Suggestion_And_No_Arguments_When_Root_Command_Is_Unknown_And_Distance_Is_Small(string expected)
            {
                // Given
                var configurator = new Configurator(new FakeTypeRegistrar(), typeof(GenericCommand<EmptySettings>));
                configurator.AddCommand<GenericCommand<EmptySettings>>("cat");

                // When
                var result = Fixture.GetParseMessage(new[] { "bat" }, configurator);

                // Then
                result.ShouldBe(expected);
            }

            [Theory]
            [EmbeddedResourceData("Spectre.Cli.Tests/Data/Resources/Exceptions/Parsing/UnknownCommand_Suggestion_NoArguments")]
            public void Should_Return_Correct_Text_With_Suggestion_And_No_Arguments_When_Command_Is_Unknown_And_Distance_Is_Small(string expected)
            {
                // Given
                var configurator = new Configurator(new FakeTypeRegistrar());
                configurator.AddBranch<CommandSettings>("dog", a =>
                {
                    a.AddCommand<CatCommand>("cat");
                });

                // When
                var result = Fixture.GetParseMessage(new[] { "dog", "bat" }, configurator);

                // Then
                result.ShouldBe(expected);
            }

            [Theory]
            [EmbeddedResourceData("Spectre.Cli.Tests/Data/Resources/Exceptions/Parsing/UnknownRootCommand_Suggestion_ArgumentBefore")]
            public void Should_Return_Correct_Text_With_Suggestion_When_Root_Command_After_Argument_Is_Unknown_And_Distance_Is_Small(string expected)
            {
                // Given
                var configurator = new Configurator(new FakeTypeRegistrar(), typeof(GenericCommand<FooCommandSettings>));
                configurator.AddCommand<GenericCommand<BarCommandSettings>>("bar");

                // When
                var result = Fixture.GetParseMessage(new[] { "qux", "bat" }, configurator);

                // Then
                result.ShouldBe(expected);
            }

            [Theory]
            [EmbeddedResourceData("Spectre.Cli.Tests/Data/Resources/Exceptions/Parsing/UnknownCommand_Suggestion_ArgumentBefore")]
            public void Should_Return_Correct_Text_With_Suggestion_When_Command_After_Argument_Is_Unknown_And_Distance_Is_Small(string expected)
            {
                // Given
                var configurator = new Configurator(new FakeTypeRegistrar());
                configurator.AddBranch<FooCommandSettings>("foo", a =>
                {
                    a.AddCommand<GenericCommand<BarCommandSettings>>("bar");
                });

                // When
                var result = Fixture.GetParseMessage(new[] { "foo", "qux", "bat" }, configurator);

                // Then
                result.ShouldBe(expected);
            }

            [Theory]
            [EmbeddedResourceData("Spectre.Cli.Tests/Data/Resources/Exceptions/Parsing/UnknownCommand_NoArguments")]
            public void Should_Return_Correct_Text_For_Unknown_Command_When_Current_Command_Has_No_Arguments(string expected)
            {
                // Given
                var configurator = new Configurator(new FakeTypeRegistrar());
                configurator.AddCommand<EmptyCommand>("empty");

                // When
                var result = Fixture.GetParseMessage(new[] { "empty", "other" }, configurator);

                // Then
                result.ShouldBe(expected);
            }
        }

        public sealed class TheCannotAssignValueToFlagMethod
        {
            [Theory]
            [EmbeddedResourceData("Spectre.Cli.Tests/Data/Resources/Exceptions/Parsing/CannotAssignValueToFlag_Long")]
            public void Should_Return_Correct_Text_For_Long_Option(string expected)
            {
                // Given
                var configurator = new Configurator(new FakeTypeRegistrar());
                configurator.AddCommand<DogCommand>("dog");

                // When
                var result = Fixture.GetParseMessage(new[] { "dog", "--alive", "foo" }, configurator);

                // Then
                result.ShouldBe(expected);
            }

            [Theory]
            [EmbeddedResourceData("Spectre.Cli.Tests/Data/Resources/Exceptions/Parsing/CannotAssignValueToFlag_Short")]
            public void Should_Return_Correct_Text_For_Short_Option(string expected)
            {
                // Given
                var configurator = new Configurator(new FakeTypeRegistrar());
                configurator.AddCommand<DogCommand>("dog");

                // When
                var result = Fixture.GetParseMessage(new[] { "dog", "-a", "foo" }, configurator);

                // Then
                result.ShouldBe(expected);
            }
        }

        public sealed class TheNoValueForOptionMethod
        {
            [Theory]
            [EmbeddedResourceData("Spectre.Cli.Tests/Data/Resources/Exceptions/Parsing/NoValueForOption_Long")]
            public void Should_Return_Correct_Text_For_Long_Option(string expected)
            {
                // Given
                var configurator = new Configurator(new FakeTypeRegistrar());
                configurator.AddCommand<DogCommand>("dog");

                // When
                var result = Fixture.GetParseMessage(new[] { "dog", "--name" }, configurator);

                // Then
                result.ShouldBe(expected);
            }

            [Theory]
            [EmbeddedResourceData("Spectre.Cli.Tests/Data/Resources/Exceptions/Parsing/NoValueForOption_Short")]
            public void Should_Return_Correct_Text_For_Short_Option(string expected)
            {
                // Given
                var configurator = new Configurator(new FakeTypeRegistrar());
                configurator.AddCommand<DogCommand>("dog");

                // When
                var result = Fixture.GetParseMessage(new[] { "dog", "-n" }, configurator);

                // Then
                result.ShouldBe(expected);
            }
        }

        public sealed class TheCouldNotMatchArgumentMethod
        {
            [Theory]
            [EmbeddedResourceData("Spectre.Cli.Tests/Data/Resources/Exceptions/Parsing/CouldNotMatchArgument")]
            public void Should_Return_Correct_Text(string expected)
            {
                // Given
                var configurator = new Configurator(new FakeTypeRegistrar());
                configurator.AddCommand<GiraffeCommand>("giraffe");

                // When
                var result = Fixture.GetParseMessage(new[] { "giraffe", "foo", "bar", "baz" }, configurator);

                // Then
                result.ShouldBe(expected);
            }
        }

        public sealed class TheUnexpectedOptionMethod
        {
            [Theory]
            [EmbeddedResourceData("Spectre.Cli.Tests/Data/Resources/Exceptions/Parsing/UnexpectedOption_Long")]
            public void Should_Return_Correct_Text_For_Long_Option(string expected)
            {
                // Given
                var configurator = new Configurator(new FakeTypeRegistrar());
                configurator.AddCommand<DogCommand>("dog");

                // When
                var result = Fixture.GetParseMessage(new[] { "--foo" }, configurator);

                // Then
                result.ShouldBe(expected);
            }

            [Theory]
            [EmbeddedResourceData("Spectre.Cli.Tests/Data/Resources/Exceptions/Parsing/UnexpectedOption_Short")]
            public void Should_Return_Correct_Text_For_Short_Option(string expected)
            {
                // Given
                var configurator = new Configurator(new FakeTypeRegistrar());
                configurator.AddCommand<DogCommand>("dog");

                // When
                var result = Fixture.GetParseMessage(new[] { "-f" }, configurator);

                // Then
                result.ShouldBe(expected);
            }
        }

        public sealed class TheUnknownOptionMethod
        {
            [Theory]
            [EmbeddedResourceData("Spectre.Cli.Tests/Data/Resources/Exceptions/Parsing/UnknownOption_Long")]
            public void Should_Return_Correct_Text_For_Long_Option_If_Strict_Mode_Is_Enabled(string expected)
            {
                // Given
                var configurator = new Configurator(new FakeTypeRegistrar());
                configurator.UseStrictParsing();
                configurator.AddCommand<DogCommand>("dog");

                // When
                var result = Fixture.GetParseMessage(new[] { "dog", "--unknown" }, configurator);

                // Then
                result.ShouldBe(expected);
            }

            [Theory]
            [EmbeddedResourceData("Spectre.Cli.Tests/Data/Resources/Exceptions/Parsing/UnknownOption_Short")]
            public void Should_Return_Correct_Text_For_Short_Option_If_Strict_Mode_Is_Enabled(string expected)
            {
                // Given
                var configurator = new Configurator(new FakeTypeRegistrar());
                configurator.UseStrictParsing();
                configurator.AddCommand<DogCommand>("dog");

                // When
                var result = Fixture.GetParseMessage(new[] { "dog", "-u" }, configurator);

                // Then
                result.ShouldBe(expected);
            }
        }

        public sealed class TheUnterminatedQuoteMethod
        {
            [Theory]
            [EmbeddedResourceData("Spectre.Cli.Tests/Data/Resources/Exceptions/Parsing/Tokenization/UnterminatedQuote")]
            public void Should_Return_Correct_Text(string expected)
            {
                // Given
                var configurator = new Configurator(new FakeTypeRegistrar());
                configurator.AddCommand<DogCommand>("dog");

                // When
                var result = Fixture.GetParseMessage(new[] { "--name", "\"Rufus" }, configurator);

                // Then
                result.ShouldBe(expected);
            }
        }

        public sealed class TheOptionWithoutNameMethod
        {
            [Theory]
            [EmbeddedResourceData("Spectre.Cli.Tests/Data/Resources/Exceptions/Parsing/Tokenization/OptionHasNoName_Short")]
            public void Should_Return_Correct_Text_For_Short_Option(string expected)
            {
                // Given
                var configurator = new Configurator(new FakeTypeRegistrar());
                configurator.AddCommand<DogCommand>("dog");

                // When
                var result = Fixture.GetParseMessage(new[] { "dog", "-", " " }, configurator);

                // Then
                result.ShouldBe(expected);
            }

            [Theory]
            [EmbeddedResourceData("Spectre.Cli.Tests/Data/Resources/Exceptions/Parsing/Tokenization/OptionValueWasExpected_Equality_Long", '=')]
            [EmbeddedResourceData("Spectre.Cli.Tests/Data/Resources/Exceptions/Parsing/Tokenization/OptionValueWasExpected_Colon_Long", ':')]
            public void Should_Return_Correct_Text_For_Expected_Long_Option_Value(string expected, char separator)
            {
                // Given
                var configurator = new Configurator(new FakeTypeRegistrar());
                configurator.AddCommand<DogCommand>("dog");

                // When
                var result = Fixture.GetParseMessage(new[] { "dog", $"--foo{separator}" }, configurator);

                // Then
                result.ShouldBe(expected);
            }

            [Theory]
            [EmbeddedResourceData("Spectre.Cli.Tests/Data/Resources/Exceptions/Parsing/Tokenization/OptionValueWasExpected_Equality_Short", '=')]
            [EmbeddedResourceData("Spectre.Cli.Tests/Data/Resources/Exceptions/Parsing/Tokenization/OptionValueWasExpected_Colon_Short", ':')]
            public void Should_Return_Correct_Text_For_Expected_Short_Option_Value(string expected, char separator)
            {
                // Given
                var configurator = new Configurator(new FakeTypeRegistrar());
                configurator.AddCommand<DogCommand>("dog");

                // When
                var result = Fixture.GetParseMessage(new[] { "dog", $"-f{separator}" }, configurator);

                // Then
                result.ShouldBe(expected);
            }
        }

        public sealed class TheInvalidShortOptionNameMethod
        {
            [Theory]
            [EmbeddedResourceData("Spectre.Cli.Tests/Data/Resources/Exceptions/Parsing/Tokenization/InvalidShortOptionName")]
            public void Should_Return_Correct_Text(string expected)
            {
                // Given
                var configurator = new Configurator(new FakeTypeRegistrar());
                configurator.AddCommand<DogCommand>("dog");

                // When
                var result = Fixture.GetParseMessage(new[] { "dog", "-f0o" }, configurator);

                // Then
                result.ShouldBe(expected);
            }
        }

        public sealed class TheLongOptionNameIsOneCharacterMethod
        {
            [Theory]
            [EmbeddedResourceData("Spectre.Cli.Tests/Data/Resources/Exceptions/Parsing/Tokenization/LongOptionNameIsOneCharacter")]
            public void Should_Return_Correct_Text(string expected)
            {
                // Given
                var configurator = new Configurator(new FakeTypeRegistrar());
                configurator.AddCommand<DogCommand>("dog");

                // When
                var result = Fixture.GetParseMessage(new[] { "dog", "--f" }, configurator);

                // Then
                result.ShouldBe(expected);
            }
        }

        public sealed class TheLongOptionNameIsMissingMethod
        {
            [Theory]
            [EmbeddedResourceData("Spectre.Cli.Tests/Data/Resources/Exceptions/Parsing/Tokenization/LongOptionNameIsMissing")]
            public void Should_Return_Correct_Text(string expected)
            {
                // Given
                var configurator = new Configurator(new FakeTypeRegistrar());
                configurator.AddCommand<DogCommand>("dog");

                // When
                var result = Fixture.GetParseMessage(new[] { "dog", "-- " }, configurator);

                // Then
                result.ShouldBe(expected);
            }
        }

        public sealed class TheLongOptionNameStartWithDigitMethod
        {
            [Theory]
            [EmbeddedResourceData("Spectre.Cli.Tests/Data/Resources/Exceptions/Parsing/Tokenization/LongOptionNameStartWithDigit")]
            public void Should_Return_Correct_Text(string expected)
            {
                // Given
                var configurator = new Configurator(new FakeTypeRegistrar());
                configurator.AddCommand<DogCommand>("dog");

                // When
                var result = Fixture.GetParseMessage(new[] { "dog", "--1foo" }, configurator);

                // Then
                result.ShouldBe(expected);
            }
        }

        public sealed class TheLongOptionNameContainSymbolMethod
        {
            [Theory]
            [EmbeddedResourceData("Spectre.Cli.Tests/Data/Resources/Exceptions/Parsing/Tokenization/LongOptionNameContainSymbol")]
            public void Should_Return_Correct_Text(string expected)
            {
                // Given
                var configurator = new Configurator(new FakeTypeRegistrar());
                configurator.AddCommand<DogCommand>("dog");

                // When
                var result = Fixture.GetParseMessage(new[] { "dog", "--f€oo" }, configurator);

                // Then
                result.ShouldBe(expected);
            }
        }

        internal static class Fixture
        {
            public static string GetParseMessage(IEnumerable<string> args, Configurator configurator)
            {
                try
                {
                    // Create the model from the configuration.
                    var model = CommandModelBuilder.Build(configurator);

                    // Parse the resulting tree from the model and the args.
                    var parser = new CommandTreeParser(model);
                    parser.Parse(args);

                    // If we get here, something is wrong.
                    throw new InvalidOperationException("Expected a parse exception.");
                }
                catch (ParseException ex)
                {
                    return Render(ex.Pretty);
                }
            }

            private static string Render(IRenderable renderable)
            {
                var builder = new StringBuilder();
                var renderer = new StringRenderer(builder);
                renderable?.Render(renderer);
                return builder.ToString().NormalizeLineEndings().Trim();
            }
        }
    }
}
