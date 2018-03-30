using Shouldly;
using Spectre.CommandLine.Internal.Templating;
using Xunit;

namespace Spectre.CommandLine.Tests.Unit.Internal.Parsing.Tokenization
{
    public sealed class TemplateTokenizerTests
    {
        [Theory]
        [InlineData("--foo", "foo")]
        [InlineData("--foo-bar", "foo-bar")]
        public void Should_Parse_Long_Option(string template, string name)
        {
            // Given, When
            var result = TemplateTokenizer.Tokenize(template);

            // Then
            result.Count.ShouldBe(1);
            result[0].TokenKind.ShouldBe(TemplateToken.Kind.LongName);
            result[0].Value.ShouldBe(name);
        }

        [Fact]
        public void Should_Parse_Short_Option()
        {
            // Given, When
            var result = TemplateTokenizer.Tokenize("-f");

            // Then
            result.Count.ShouldBe(1);
            result[0].TokenKind.ShouldBe(TemplateToken.Kind.ShortName);
            result[0].Value.ShouldBe("f");
        }

        [Fact]
        public void Should_Parse_Multiple_Options()
        {
            // Given, When
            var result = TemplateTokenizer.Tokenize("-f|--foo|--bar|-b");

            // Then
            result.Count.ShouldBe(4);
            result[0].TokenKind.ShouldBe(TemplateToken.Kind.ShortName);
            result[0].Value.ShouldBe("f");
            result[1].TokenKind.ShouldBe(TemplateToken.Kind.LongName);
            result[1].Value.ShouldBe("foo");
            result[2].TokenKind.ShouldBe(TemplateToken.Kind.LongName);
            result[2].Value.ShouldBe("bar");
            result[3].TokenKind.ShouldBe(TemplateToken.Kind.ShortName);
            result[3].Value.ShouldBe("b");
        }

        [Theory]
        [InlineData("<FOO>", "FOO")]
        public void Should_Parse_Required_Value(string template, string name)
        {
            // Given, When
            var result = TemplateTokenizer.Tokenize(template);

            // Then
            result.Count.ShouldBe(1);
            result[0].TokenKind.ShouldBe(TemplateToken.Kind.RequiredValue);
            result[0].Value.ShouldBe(name);
        }

        [Theory]
        [InlineData("[FOO]", "FOO")]
        public void Should_Parse_Optional_Value(string template, string name)
        {
            // Given, When
            var result = TemplateTokenizer.Tokenize(template);

            // Then
            result.Count.ShouldBe(1);
            result[0].TokenKind.ShouldBe(TemplateToken.Kind.OptionalValue);
            result[0].Value.ShouldBe(name);
        }

        [Theory]
        [InlineData("--long [FOO")]
        [InlineData("--long <FOO")]
        public void Should_Throw_If_Value_Is_Unterminated(string template)
        {
            // Given, When
            var result = Record.Exception(() => TemplateTokenizer.Tokenize(template));

            // Then
            result.ShouldBeOfType<TemplateException>().And(e =>
            {
                e.Message.ShouldBe("Encountered unterminated value name 'FOO'.");
                e.Template.ShouldBe(template);
            });
        }
    }
}
