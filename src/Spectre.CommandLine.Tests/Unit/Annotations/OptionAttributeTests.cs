using System;
using Shouldly;
using Xunit;

namespace Spectre.CommandLine.Tests.Unit.Annotations
{
    public sealed class OptionAttributeTests
    {
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
            option.LongName.ShouldBe("option");
        }

        [Theory]
        [InlineData("<VALUE>", true)]
        [InlineData("[VALUE]", false)]
        public void Should_Parse_Requirement_Correctly(string value, bool expected)
        {
            // Given, When
            var option = new CommandOptionAttribute($"-o|--option {value}");

            // Then
            option.IsRequired.ShouldBe(expected);
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
            option.LongName.ShouldBe("option");
        }

        [Theory]
        [InlineData("")]
        [InlineData("<VALUE>")]
        public void Should_Throw_If_Template_Is_Empty(string value)
        {
            // Given, When
            var option = Record.Exception(() => new CommandOptionAttribute(value));

            // Then
            option.ShouldBeOfType<InvalidOperationException>().And(e =>
            {
                e.Message.ShouldBe("No long or short name for option has been specified.");
            });
        }

        [Theory]
        [InlineData("-option")]
        [InlineData("-")]
        public void Should_Throw_If_Short_Name_Is_Invalid(string value)
        {
            // Given, When
            var option = Record.Exception(() => new CommandOptionAttribute(value));

            // Then
            option.ShouldBeOfType<CommandAppException>().And(e =>
            {
                e.Message.ShouldBe("Invalid short option.");
            });
        }

        [Theory]
        [InlineData("--o")]
        [InlineData("--")]
        public void Should_Throw_If_Long_Name_Is_Invalid(string value)
        {
            // Given, When
            var option = Record.Exception(() => new CommandOptionAttribute(value));

            // Then
            option.ShouldBeOfType<CommandAppException>().And(e =>
            {
                e.Message.ShouldBe("Invalid long option.");
            });
        }
    }
}
