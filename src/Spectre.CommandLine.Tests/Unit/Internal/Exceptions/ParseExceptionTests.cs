using System;
using System.Collections.Generic;
using System.Text;
using Shouldly;
using Spectre.CommandLine.Internal.Configuration;
using Spectre.CommandLine.Internal.Exceptions;
using Spectre.CommandLine.Internal.Modelling;
using Spectre.CommandLine.Internal.Parsing;
using Spectre.CommandLine.Internal.Rendering;
using Spectre.CommandLine.Tests.Data;
using Spectre.CommandLine.Tests.Fakes;
using Spectre.CommandLine.Tests.Utilities;
using Xunit;

namespace Spectre.CommandLine.Tests.Unit.Internal.Exceptions
{
    public static class ParseExceptionTests
    {
        public sealed class UnknownCommand
        {
            [Theory]
            [EmbeddedResourceData("Spectre.CommandLine.Tests/Data/Resources/Exceptions/Parsing/UnknownCommand")]
            public void Should_Return_Correct_Text_When_First_Command_Is_Unknown(string expected)
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
            [EmbeddedResourceData("Spectre.CommandLine.Tests/Data/Resources/Exceptions/Parsing/UnknownCommand_NoArguments")]
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
            [EmbeddedResourceData("Spectre.CommandLine.Tests/Data/Resources/Exceptions/Parsing/CannotAssignValueToFlag_Long")]
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
            [EmbeddedResourceData("Spectre.CommandLine.Tests/Data/Resources/Exceptions/Parsing/CannotAssignValueToFlag_Short")]
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
            [EmbeddedResourceData("Spectre.CommandLine.Tests/Data/Resources/Exceptions/Parsing/NoValueForOption_Long")]
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
            [EmbeddedResourceData("Spectre.CommandLine.Tests/Data/Resources/Exceptions/Parsing/NoValueForOption_Short")]
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
            [EmbeddedResourceData("Spectre.CommandLine.Tests/Data/Resources/Exceptions/Parsing/CouldNotMatchArgument")]
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
            [EmbeddedResourceData("Spectre.CommandLine.Tests/Data/Resources/Exceptions/Parsing/UnexpectedOption_Long")]
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
            [EmbeddedResourceData("Spectre.CommandLine.Tests/Data/Resources/Exceptions/Parsing/UnexpectedOption_Short")]
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

        public sealed class TheUnterminatedQuoteMethod
        {
            [Theory]
            [EmbeddedResourceData("Spectre.CommandLine.Tests/Data/Resources/Exceptions/Parsing/Tokenization/UnterminatedQuote")]
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
            [EmbeddedResourceData("Spectre.CommandLine.Tests/Data/Resources/Exceptions/Parsing/Tokenization/OptionHasNoName_Short")]
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
            [EmbeddedResourceData("Spectre.CommandLine.Tests/Data/Resources/Exceptions/Parsing/Tokenization/OptionHasNoName_Long")]
            public void Should_Return_Correct_Text_For_Long_Option(string expected)
            {
                // Given
                var configurator = new Configurator(new FakeTypeRegistrar());
                configurator.AddCommand<DogCommand>("dog");

                // When
                var result = Fixture.GetParseMessage(new[] { "dog", "--" }, configurator);

                // Then
                result.ShouldBe(expected);
            }
        }

        public sealed class TheInvalidShortOptionNameMethod
        {
            [Theory]
            [EmbeddedResourceData("Spectre.CommandLine.Tests/Data/Resources/Exceptions/Parsing/Tokenization/InvalidShortOptionName")]
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
            [EmbeddedResourceData("Spectre.CommandLine.Tests/Data/Resources/Exceptions/Parsing/Tokenization/LongOptionNameIsOneCharacter")]
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
            [EmbeddedResourceData("Spectre.CommandLine.Tests/Data/Resources/Exceptions/Parsing/Tokenization/LongOptionNameIsMissing")]
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
            [EmbeddedResourceData("Spectre.CommandLine.Tests/Data/Resources/Exceptions/Parsing/Tokenization/LongOptionNameStartWithDigit")]
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
            [EmbeddedResourceData("Spectre.CommandLine.Tests/Data/Resources/Exceptions/Parsing/Tokenization/LongOptionNameContainSymbol")]
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
