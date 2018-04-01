using System;
using System.Text;
using Shouldly;
using Spectre.CommandLine.Internal.Exceptions;
using Spectre.CommandLine.Internal.Rendering;
using Spectre.CommandLine.Internal.Templating;
using Spectre.CommandLine.Tests.Utilities;
using Xunit;

namespace Spectre.CommandLine.Tests.Unit.Internal.Exceptions
{
    public sealed class TemplateExceptionTests
    {
        [Theory]
        [EmbeddedResourceData("Spectre.CommandLine.Tests/Data/Resources/Exceptions/Template/UnterminatedValueName")]
        public void The_UnterminatedValueName_Method_Should_Return_Correct_Text(string expected)
        {
            // Given, When
            var message = Fixture.GetArgumentTemplateParsingMessage("--foo|-f <BAR");

            // Then
            message.ShouldBe(expected);
        }

        [Theory]
        [EmbeddedResourceData("Spectre.CommandLine.Tests/Data/Resources/Exceptions/Template/ArgumentCannotContainOptions")]
        public void The_ArgumentCannotContainOptions_Method_Should_Return_Correct_Text(string expected)
        {
            // Given, When
            var message = Fixture.GetArgumentTemplateParsingMessage("--foo <BAR>");

            // Then
            message.ShouldBe(expected);
        }

        [Theory]
        [EmbeddedResourceData("Spectre.CommandLine.Tests/Data/Resources/Exceptions/Template/MultipleValuesAreNotSupported")]
        public void The_MultipleValuesAreNotSupported_Method_Should_Return_Correct_Text(string expected)
        {
            // Given, When
            var message = Fixture.GetArgumentTemplateParsingMessage("<FOO> <BAR>");

            // Then
            message.ShouldBe(expected);
        }

        [Theory]
        [EmbeddedResourceData("Spectre.CommandLine.Tests/Data/Resources/Exceptions/Template/ValuesMustHaveName")]
        public void The_ValuesMustHaveName_Method_Should_Return_Correct_Text(string expected)
        {
            // Given, When
            var message = Fixture.GetArgumentTemplateParsingMessage("<>");

            // Then
            message.ShouldBe(expected);
        }

        [Theory]
        [EmbeddedResourceData("Spectre.CommandLine.Tests/Data/Resources/Exceptions/Template/OptionsMustHaveName")]
        public void The_OptionsMustHaveName_Method_Should_Return_Correct_Text(string expected)
        {
            // Given, When
            var message = Fixture.GetOptionTemplateParsingMessage("--foo|-");

            // Then
            message.ShouldBe(expected);
        }

        [Theory]
        [EmbeddedResourceData("Spectre.CommandLine.Tests/Data/Resources/Exceptions/Template/OptionNamesCannotStartWithDigit")]
        public void The_OptionNamesCannotStartWithDigit_Method_Should_Return_Correct_Text(string expected)
        {
            // Given, When
            var message = Fixture.GetOptionTemplateParsingMessage("--1foo");

            // Then
            message.ShouldBe(expected);
        }

        [Theory]
        [EmbeddedResourceData("Spectre.CommandLine.Tests/Data/Resources/Exceptions/Template/InvalidCharacterInOptionName")]
        public void The_InvalidCharacterInOptionName_Method_Should_Return_Correct_Text(string expected)
        {
            // Given, When
            var message = Fixture.GetOptionTemplateParsingMessage("--f$oo");

            // Then
            message.ShouldBe(expected);
        }

        [Theory]
        [EmbeddedResourceData("Spectre.CommandLine.Tests/Data/Resources/Exceptions/Template/MultipleLongOptionNamesNotAllowed")]
        public void The_MultipleLongOptionNamesNotAllowed_Method_Should_Return_Correct_Text(string expected)
        {
            // Given, When
            var message = Fixture.GetOptionTemplateParsingMessage("--foo|--bar");

            // Then
            message.ShouldBe(expected);
        }

        [Theory]
        [EmbeddedResourceData("Spectre.CommandLine.Tests/Data/Resources/Exceptions/Template/LongOptionMustHaveMoreThanOneCharacter")]
        public void The_LongOptionMustHaveMoreThanOneCharacter_Method_Should_Return_Correct_Text(string expected)
        {
            // Given, When
            var message = Fixture.GetOptionTemplateParsingMessage("--f");

            // Then
            message.ShouldBe(expected);
        }

        [Theory]
        [EmbeddedResourceData("Spectre.CommandLine.Tests/Data/Resources/Exceptions/Template/MultipleShortOptionNamesNotAllowed")]
        public void The_MultipleShortOptionNamesNotAllowed_Method_Should_Return_Correct_Text(string expected)
        {
            // Given, When
            var message = Fixture.GetOptionTemplateParsingMessage("-f|-b");

            // Then
            message.ShouldBe(expected);
        }

        [Theory]
        [EmbeddedResourceData("Spectre.CommandLine.Tests/Data/Resources/Exceptions/Template/ShortOptionMustOnlyBeOneCharacter")]
        public void The_ShortOptionMustOnlyBeOneCharacter_Method_Should_Return_Correct_Text(string expected)
        {
            // Given, When
            var message = Fixture.GetOptionTemplateParsingMessage("--foo|-bar");

            // Then
            message.ShouldBe(expected);
        }

        [Theory]
        [EmbeddedResourceData("Spectre.CommandLine.Tests/Data/Resources/Exceptions/Template/MultipleOptionValuesAreNotSupported")]
        public void The_MultipleOptionValuesAreNotSupported_Method_Should_Return_Correct_Text(string expected)
        {
            // Given, When
            var message = Fixture.GetOptionTemplateParsingMessage("-f|--foo <FOO> <BAR>");

            // Then
            message.ShouldBe(expected);
        }

        [Theory]
        [EmbeddedResourceData("Spectre.CommandLine.Tests/Data/Resources/Exceptions/Template/OptionValueCannotBeOptional")]
        public void The_OptionValueCannotBeOptional_Method_Should_Return_Correct_Text(string expected)
        {
            // Given, When
            var message = Fixture.GetOptionTemplateParsingMessage("-f|--foo [FOO]");

            // Then
            message.ShouldBe(expected);
        }

        [Theory]
        [EmbeddedResourceData("Spectre.CommandLine.Tests/Data/Resources/Exceptions/Template/InvalidCharacterInValueName")]
        public void The_InvalidCharacterInValueName_Method_Should_Return_Correct_Text(string expected)
        {
            // Given, When
            var message = Fixture.GetOptionTemplateParsingMessage("-f|--foo <F$OO>");

            // Then
            message.ShouldBe(expected);
        }

        [Theory]
        [EmbeddedResourceData("Spectre.CommandLine.Tests/Data/Resources/Exceptions/Template/MissingLongAndShortName")]
        public void The_MissingLongAndShortName_Method_Should_Return_Correct_Text(string expected)
        {
            // Given, When
            var message = Fixture.GetOptionTemplateParsingMessage("<FOO>");

            // Then
            message.ShouldBe(expected);
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
