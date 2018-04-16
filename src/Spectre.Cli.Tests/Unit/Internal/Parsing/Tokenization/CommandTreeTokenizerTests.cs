using Shouldly;
using Spectre.Cli.Internal.Parsing.Tokenization;
using Xunit;

namespace Spectre.Cli.Tests.Unit.Internal.Parsing.Tokenization
{
    public sealed class CommandTreeTokenizerTests
    {
        [Theory]
        [InlineData("foo")]
        public void Should_Parse_String_Correctly(params string[] args)
        {
            // Given, When
            var result = CommandTreeTokenizer.Tokenize(args);

            // Then
            result.Count.ShouldBe(1);
            result[0].TokenKind.ShouldBe(CommandTreeToken.Kind.String);
            result[0].Value.ShouldBe("foo");
        }

        [Theory]
        [InlineData("\"foo\"")]
        public void Should_Parse_Quoted_String_Correctly(params string[] args)
        {
            // Given, When
            var result = CommandTreeTokenizer.Tokenize(args);

            // Then
            result.Count.ShouldBe(1);
            result[0].TokenKind.ShouldBe(CommandTreeToken.Kind.String);
            result[0].Value.ShouldBe("foo");
        }

        [Theory]
        [InlineData("-f", "bar")]
        [InlineData("-f=bar")]
        [InlineData("-f=\"bar\"")]
        [InlineData("-f:bar")]
        [InlineData("-f:\"bar\"")]
        public void Should_Parse_Short_Option_Correctly(params string[] args)
        {
            // Given, When
            var result = CommandTreeTokenizer.Tokenize(args);

            // Then
            result.Count.ShouldBe(2);
            result[0].TokenKind.ShouldBe(CommandTreeToken.Kind.ShortOption);
            result[0].Value.ShouldBe("f");
            result[1].TokenKind.ShouldBe(CommandTreeToken.Kind.String);
            result[1].Value.ShouldBe("bar");
        }

        [Theory]
        [InlineData("--foo", "bar")]
        [InlineData("--foo=bar")]
        [InlineData("--foo=\"bar\"")]
        [InlineData("--foo:bar")]
        [InlineData("--foo:\"bar\"")]
        public void Should_Parse_Long_Option_Correctly(params string[] args)
        {
            // Given, When
            var result = CommandTreeTokenizer.Tokenize(args);

            // Then
            result.Count.ShouldBe(2);
            result[0].TokenKind.ShouldBe(CommandTreeToken.Kind.LongOption);
            result[0].Value.ShouldBe("foo");
            result[1].TokenKind.ShouldBe(CommandTreeToken.Kind.String);
            result[1].Value.ShouldBe("bar");
        }
    }
}
