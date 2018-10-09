using Shouldly;
using Spectre.Cli.Internal.Exceptions;
using Xunit;

namespace Spectre.Cli.Tests.Unit.Annotations
{
    public sealed class OptionAttributeTests
    {
        [Fact]
        public void Should_Throw_If_Value_Name_Is_Marked_As_Optional()
        {
            // Given, When
            var result = Record.Exception(() => new CommandOptionAttribute("-o|--option [VALUE]"));

            // Then
            result.ShouldNotBe(null);
            result.ShouldBeOfType<TemplateException>().And(exception =>
                exception.Message.ShouldBe("Option values cannot be optional."));
        }

        [Fact]
        public void Should_Parse_Short_Name_Correctly()
        {
            // Given, When
            var option = new CommandOptionAttribute("-o|--option <VALUE>");

            // Then
            option.ShortName.ShouldBe("o");
        }

        [Fact]
        public void Should_Parse_Long_Name_Correctly()
        {
            // Given, When
            var option = new CommandOptionAttribute("-o|--option <VALUE>");

            // Then
            option.LongNames.ShouldContain("option");
        }

        [Theory]
        [InlineData("<VALUE>")]
        public void Should_Parse_Value_Correctly(string value)
        {
            // Given, When
            var option = new CommandOptionAttribute($"-o|--option {value}");

            // Then
            option.ValueName.ShouldBe("VALUE");
        }

        [Fact]
        public void Should_Parse_Only_Short_Name()
        {
            // Given, When
            var option = new CommandOptionAttribute("-o");

            // Then
            option.ShortName.ShouldBe("o");
        }

        [Fact]
        public void Should_Parse_Only_Long_Name()
        {
            // Given, When
            var option = new CommandOptionAttribute("--option");

            // Then
            option.LongNames.ShouldContain("option");
        }

        [Theory]
        [InlineData("")]
        [InlineData("<VALUE>")]
        public void Should_Throw_If_Template_Is_Empty(string value)
        {
            // Given, When
            var option = Record.Exception(() => new CommandOptionAttribute(value));

            // Then
            option.ShouldBeOfType<TemplateException>().And(e =>
                e.Message.ShouldBe("No long or short name for option has been specified."));
        }

        [Theory]
        [InlineData("-option")]
        public void Should_Throw_If_Short_Name_Is_Invalid(string value)
        {
            // Given, When
            var option = Record.Exception(() => new CommandOptionAttribute(value));

            // Then
            option.ShouldBeOfType<TemplateException>().And(e =>
                e.Message.ShouldBe("Short option names can not be longer than one character."));
        }

        [Theory]
        [InlineData("--o")]
        public void Should_Throw_If_Long_Name_Is_Invalid(string value)
        {
            // Given, When
            var option = Record.Exception(() => new CommandOptionAttribute(value));

            // Then
            option.ShouldBeOfType<TemplateException>().And(e =>
                e.Message.ShouldBe("Long option names must consist of more than one character."));
        }

        [Theory]
        [InlineData("-")]
        [InlineData("--")]
        public void Should_Throw_If_Option_Have_No_Name(string template)
        {
            // Given, When
            var option = Record.Exception(() => new CommandOptionAttribute(template));

            // Then
            option.ShouldBeOfType<TemplateException>().And(e =>
                e.Message.ShouldBe("Options without name are not allowed."));
        }
    }
}
