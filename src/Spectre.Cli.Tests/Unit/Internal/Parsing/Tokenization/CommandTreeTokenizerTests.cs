using Shouldly;
using Spectre.Cli.Internal.Parsing;
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

        [Fact]
        public void Should_Parse_Remaining_Parameters()
        {
            // Given, When
            var result = CommandTreeTokenizer.Tokenize(new[] { "--foo", "--", "--bar", "-qux", "\"lol\"", "w00t" });

            // Then
            result.Count.ShouldBe(5);
            result[0].TokenKind.ShouldBe(CommandTreeToken.Kind.LongOption);
            result[0].Value.ShouldBe("foo");
            result[1].TokenKind.ShouldBe(CommandTreeToken.Kind.Remaining);
            result[1].Value.ShouldBe("--bar");
            result[2].TokenKind.ShouldBe(CommandTreeToken.Kind.Remaining);
            result[2].Value.ShouldBe("-qux");
            result[3].TokenKind.ShouldBe(CommandTreeToken.Kind.Remaining);
            result[3].Value.ShouldBe("\"lol\"");
            result[4].TokenKind.ShouldBe(CommandTreeToken.Kind.Remaining);
            result[4].Value.ShouldBe("w00t");
        }
    }
}
