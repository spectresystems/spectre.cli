using System;
using System.Text;
using Shouldly;
using Spectre.Cli.Internal.Configuration;
using Spectre.Cli.Internal.Exceptions;
using Spectre.Cli.Internal.Rendering;
using Spectre.Cli.Tests.Utilities;
using Xunit;

namespace Spectre.Cli.Tests.Unit.Internal.Exceptions
{
    public static class TemplateExceptionTests
    {
        public sealed class TheUnexpectedCharacterMethod
        {
            [Theory]
            [EmbeddedResourceData("Spectre.Cli.Tests/Data/Resources/Exceptions/Template/UnexpectedCharacter")]
            public void Should_Return_Correct_Text(string expected)
            {
                // Given, When
                var message = Fixture.GetArgumentTemplateParsingMessage("<FOO> $ <BAR>");

                // Then
                message.ShouldBe(expected);
            }
        }

        public sealed class TheUnterminatedValueNameMethod
        {
            [Theory]
            [EmbeddedResourceData("Spectre.Cli.Tests/Data/Resources/Exceptions/Template/UnterminatedValueName")]
            public void Should_Return_Correct_Text(string expected)
            {
                // Given, When
                var message = Fixture.GetArgumentTemplateParsingMessage("--foo|-f <BAR");

                // Then
                message.ShouldBe(expected);
            }
        }

        public sealed class TheArgumentCannotContainOptionsMethod
        {
            [Theory]
            [EmbeddedResourceData("Spectre.Cli.Tests/Data/Resources/Exceptions/Template/ArgumentCannotContainOptions")]
            public void Should_Return_Correct_Text(string expected)
            {
                // Given, When
                var message = Fixture.GetArgumentTemplateParsingMessage("--foo <BAR>");

                // Then
                message.ShouldBe(expected);
            }
        }

        public sealed class TheMultipleValuesAreNotSupportedMethod
        {
            [Theory]
            [EmbeddedResourceData("Spectre.Cli.Tests/Data/Resources/Exceptions/Template/MultipleValuesAreNotSupported")]
            public void Should_Return_Correct_Text(string expected)
            {
                // Given, When
                var message = Fixture.GetArgumentTemplateParsingMessage("<FOO> <BAR>");

                // Then
                message.ShouldBe(expected);
            }
        }

        public sealed class TheValuesMustHaveNameMethod
        {
            [Theory]
            [EmbeddedResourceData("Spectre.Cli.Tests/Data/Resources/Exceptions/Template/ValuesMustHaveName")]
            public void Should_Return_Correct_Text(string expected)
            {
                // Given, When
                var message = Fixture.GetArgumentTemplateParsingMessage("<>");

                // Then
                message.ShouldBe(expected);
            }
        }

        public sealed class TheOptionsMustHaveNameMethod
        {
            [Theory]
            [EmbeddedResourceData("Spectre.Cli.Tests/Data/Resources/Exceptions/Template/OptionsMustHaveName")]
            public void Should_Return_Correct_Text(string expected)
            {
                // Given, When
                var message = Fixture.GetOptionTemplateParsingMessage("--foo|-");

                // Then
                message.ShouldBe(expected);
            }
        }

        public sealed class TheOptionNamesCannotStartWithDigitMethod
        {
            [Theory]
            [EmbeddedResourceData("Spectre.Cli.Tests/Data/Resources/Exceptions/Template/OptionNamesCannotStartWithDigit")]
            public void Should_Return_Correct_Text(string expected)
            {
                // Given, When
                var message = Fixture.GetOptionTemplateParsingMessage("--1foo");

                // Then
                message.ShouldBe(expected);
            }
        }

        public sealed class TheInvalidCharacterInOptionNameMethod
        {
            [Theory]
            [EmbeddedResourceData("Spectre.Cli.Tests/Data/Resources/Exceptions/Template/InvalidCharacterInOptionName")]
            public void Should_Return_Correct_Text(string expected)
            {
                // Given, When
                var message = Fixture.GetOptionTemplateParsingMessage("--f$oo");

                // Then
                message.ShouldBe(expected);
            }
        }

        public sealed class TheLongOptionMustHaveMoreThanOneCharacterMethod
        {
            [Theory]
            [EmbeddedResourceData("Spectre.Cli.Tests/Data/Resources/Exceptions/Template/LongOptionMustHaveMoreThanOneCharacter")]
            public void Should_Return_Correct_Text(string expected)
            {
                // Given, When
                var message = Fixture.GetOptionTemplateParsingMessage("--f");

                // Then
                message.ShouldBe(expected);
            }
        }

        public sealed class TheShortOptionMustOnlyBeOneCharacterMethod
        {
            [Theory]
            [EmbeddedResourceData("Spectre.Cli.Tests/Data/Resources/Exceptions/Template/ShortOptionMustOnlyBeOneCharacter")]
            public void Should_Return_Correct_Text(string expected)
            {
                // Given, When
                var message = Fixture.GetOptionTemplateParsingMessage("--foo|-bar");

                // Then
                message.ShouldBe(expected);
            }
        }

        public sealed class TheMultipleOptionValuesAreNotSupportedMethod
        {
            [Theory]
            [EmbeddedResourceData("Spectre.Cli.Tests/Data/Resources/Exceptions/Template/MultipleOptionValuesAreNotSupported")]
            public void Should_Return_Correct_Text(string expected)
            {
                // Given, When
                var message = Fixture.GetOptionTemplateParsingMessage("-f|--foo <FOO> <BAR>");

                // Then
                message.ShouldBe(expected);
            }
        }

        public sealed class TheOptionValueCannotBeOptionalMethod
        {
            [Theory]
            [EmbeddedResourceData("Spectre.Cli.Tests/Data/Resources/Exceptions/Template/OptionValueCannotBeOptional")]
            public void Should_Return_Correct_Text(string expected)
            {
                // Given, When
                var message = Fixture.GetOptionTemplateParsingMessage("-f|--foo [FOO]");

                // Then
                message.ShouldBe(expected);
            }
        }

        public sealed class TheInvalidCharacterInValueNameMethod
        {
            [Theory]
            [EmbeddedResourceData("Spectre.Cli.Tests/Data/Resources/Exceptions/Template/InvalidCharacterInValueName")]
            public void Should_Return_Correct_Text(string expected)
            {
                // Given, When
                var message = Fixture.GetOptionTemplateParsingMessage("-f|--foo <F$OO>");

                // Then
                message.ShouldBe(expected);
            }
        }

        public sealed class TheMissingLongAndShortNameMethod
        {
            [Theory]
            [EmbeddedResourceData("Spectre.Cli.Tests/Data/Resources/Exceptions/Template/MissingLongAndShortName")]
            public void Should_Return_Correct_Text(string expected)
            {
                // Given, When
                var message = Fixture.GetOptionTemplateParsingMessage("<FOO>");

                // Then
                message.ShouldBe(expected);
            }
        }

        internal static class Fixture
        {
            public static string GetOptionTemplateParsingMessage(string template)
            {
                try
                {
                    TemplateParser.ParseOptionTemplate(template);
                    throw new InvalidOperationException("Expected a template exception.");
                }
                catch (TemplateException ex)
                {
                    return Render(ex.Pretty);
                }
            }

            public static string GetArgumentTemplateParsingMessage(string template)
            {
                try
                {
                    TemplateParser.ParseArgumentTemplate(template);
                    throw new InvalidOperationException("Expected a template exception.");
                }
                catch (TemplateException ex)
                {
                    return Render(ex.Pretty);
                }
            }

            private static string Render(IRenderable renderable)
            {
                var builder = new StringBuilder();
                var renderer = new StringRenderer(builder);
                renderable.Render(renderer);
                return builder.ToString().NormalizeLineEndings().Trim();
            }
        }
    }
}
