using Shouldly;
using Spectre.CommandLine.Internal.Parsing;
using Xunit;

namespace Spectre.CommandLine.Tests.Unit.Internal.Parsing
{
    public static class TemplateParserTests
    {
        public sealed class TheParseArgumentTemplateMethod
        {
            [Theory]
            [InlineData("<FOO>", true)]
            [InlineData("[FOO]", false)]
            public void Should_Parse_Valid_Options(string template, bool required)
            {
                // Given, When
                var result = TemplateParser.ParseArgumentTemplate(template);

                // Then
                result.Value.ShouldBe("FOO");
                result.Required.ShouldBe(required);
            }

            [Theory]
            [InlineData("<FOO> <BAR>")]
            [InlineData("[FOO] [BAR]")]
            [InlineData("[FOO] <BAR>")]
            [InlineData("<FOO> [BAR]")]
            public void Should_Throw_If_Multiple_Values_Are_Provided(string template)
            {
                // Given, When
                var result = Record.Exception(() => TemplateParser.ParseArgumentTemplate(template));

                // Then
                result.ShouldBeOfType<CommandAppException>().And(e =>
                {
                    e.Message.ShouldBe("Multiple values are not supported.");
                });
            }

            [Fact]
            public void Should_Throw_If_Short_Option_Is_Provided()
            {
                // Given, When
                var result = Record.Exception(() => TemplateParser.ParseArgumentTemplate("-f"));

                // Then
                result.ShouldBeOfType<CommandAppException>().And(e =>
                {
                    e.Message.ShouldBe("Arguments can not contain options.");
                });
            }

            [Fact]
            public void Should_Throw_If_Long_Option_Is_Provided()
            {
                // Given, When
                var result = Record.Exception(() => TemplateParser.ParseArgumentTemplate("--foo"));

                // Then
                result.ShouldBeOfType<CommandAppException>().And(e =>
                {
                    e.Message.ShouldBe("Arguments can not contain options.");
                });
            }

            [Theory]
            [InlineData("[]")]
            [InlineData("<>")]
            public void Should_Throw_If_Value_Contains_No_Name(string template)
            {
                // Given, When
                var result = Record.Exception(() => TemplateParser.ParseArgumentTemplate(template));

                // Then
                result.ShouldBeOfType<CommandAppException>().And(e =>
                {
                    e.Message.ShouldBe("Values without name are not allowed.");
                });
            }
        }

        public sealed class TheParseOptionTemplateMethod
        {
            [Theory]
            [InlineData("-f|--foo <BAR>")]
            [InlineData("--foo|-f <BAR>")]
            [InlineData("<BAR> --foo|-f")]
            [InlineData("<BAR> -f|--foo")]
            [InlineData("-f <BAR> --foo")]
            [InlineData("--foo <BAR> -f")]
            public void Template_Parts_Can_Appear_In_Any_Order(string template)
            {
                // Given, When
                var result = TemplateParser.ParseOptionTemplate(template);

                // Then
                result.LongName.ShouldBe("foo");
                result.ShortName.ShouldBe("f");
                result.Value.ShouldBe("BAR");
            }

            [Theory]
            [InlineData("--|--foo")]
            [InlineData("-|--foo")]
            public void Should_Throw_If_Option_Contains_No_Name(string template)
            {
                // Given, When
                var result = Record.Exception(() => TemplateParser.ParseOptionTemplate(template));

                // Then
                result.ShouldBeOfType<CommandAppException>().And(e =>
                {
                    e.Message.ShouldBe("Options without name are not allowed.");
                });
            }

            [Fact]
            public void Should_Throw_If_Option_Value_Is_Optional()
            {
                // Given, When
                var result = Record.Exception(() => TemplateParser.ParseOptionTemplate("[FOO]"));

                // Then
                result.ShouldBeOfType<CommandAppException>().And(e =>
                {
                    e.Message.ShouldBe("Option values cannot be optional.");
                });
            }

            [Fact]
            public void Should_Throw_If_Multiple_Long_Options_Are_Provided()
            {
                // Given, When
                var result = Record.Exception(() => TemplateParser.ParseOptionTemplate("--foo|--bar"));

                // Then
                result.ShouldBeOfType<CommandAppException>().And(e =>
                {
                    e.Message.ShouldBe("Multiple long option names are not supported.");
                });
            }

            [Fact]
            public void Should_Throw_If_Multiple_Short_Options_Are_Provided()
            {
                // Given, When
                var result = Record.Exception(() => TemplateParser.ParseOptionTemplate("-f|-b"));

                // Then
                result.ShouldBeOfType<CommandAppException>().And(e =>
                {
                    e.Message.ShouldBe("Multiple short option names are not supported.");
                });
            }

            [Fact]
            public void Should_Throw_If_Multiple_Option_Values_Are_Provided()
            {
                // Given, When
                var result = Record.Exception(() => TemplateParser.ParseOptionTemplate("<FOO> <BAR>"));

                // Then
                result.ShouldBeOfType<CommandAppException>().And(e =>
                {
                    e.Message.ShouldBe("Multiple option values are not supported.");
                });
            }

            [Theory]
            [InlineData("-foo")]
            [InlineData("-f-b")]
            public void Should_Throw_If_Short_Option_Is_Longer_Than_One_Character(string template)
            {
                // Given, When
                var result = Record.Exception(() => TemplateParser.ParseOptionTemplate(template));

                // Then
                result.ShouldBeOfType<CommandAppException>().And(e =>
                {
                    e.Message.ShouldBe("Short option names can not be longer than one character.");
                });
            }

            [Theory]
            [InlineData("-1")]
            [InlineData("--1f")]
            public void Should_Throw_If_First_Letter_Of_An_Option_Name_Is_A_Digit(string template)
            {
                // Given, When
                var result = Record.Exception(() => TemplateParser.ParseOptionTemplate(template));

                // Then
                result.ShouldBeOfType<CommandAppException>().And(e =>
                {
                    e.Message.ShouldBe("Option names cannot start with a digit.");
                });
            }

            [Theory]
            [InlineData("-foo[b", '[')]
            [InlineData("-f€b", '€')]
            [InlineData("-foo@b", '@')]
            public void Should_Throw_If_Option_Contains_Invalid_Name(string template, char invalid)
            {
                // Given, When
                var result = Record.Exception(() => TemplateParser.ParseOptionTemplate(template));

                // Then
                result.ShouldBeOfType<CommandAppException>().And(e =>
                {
                    e.Message.ShouldBe($"Encountered invalid character '{invalid}' in option name.");
                });
            }

            [Theory]
            [InlineData("<FO£O>", '£')]
            [InlineData("<FOO BAR>", ' ')]
            public void Should_Throw_If_Value_Contains_Invalid_Name(string template, char invalid)
            {
                // Given, When
                var result = Record.Exception(() => TemplateParser.ParseOptionTemplate(template));

                // Then
                result.ShouldBeOfType<CommandAppException>().And(e =>
                {
                    e.Message.ShouldBe($"Encountered invalid character '{invalid}' in value name.");
                });
            }

            [Theory]
            [InlineData("<FOO> <BAR>")]
            public void Should_Throw_If_Multiple_Values_Are_Provided(string template)
            {
                // Given, When
                var result = Record.Exception(() => TemplateParser.ParseOptionTemplate(template));

                // Then
                result.ShouldBeOfType<CommandAppException>().And(e =>
                {
                    e.Message.ShouldBe("Multiple option values are not supported.");
                });
            }
        }
    }
}
