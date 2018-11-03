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
            result.Tokens.Count.ShouldBe(1);
            result.Tokens[0].TokenKind.ShouldBe(CommandTreeToken.Kind.String);
            result.Tokens[0].Value.ShouldBe("foo");
        }

        [Theory]
        [InlineData("\"foo\"")]
        public void Should_Parse_Quoted_String_Correctly(params string[] args)
        {
            // Given, When
            var result = CommandTreeTokenizer.Tokenize(args);

            // Then
            result.Tokens.Count.ShouldBe(1);
            result.Tokens[0].TokenKind.ShouldBe(CommandTreeToken.Kind.String);
            result.Tokens[0].Value.ShouldBe("foo");
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
            result.Tokens.Count.ShouldBe(2);
            result.Tokens[0].TokenKind.ShouldBe(CommandTreeToken.Kind.ShortOption);
            result.Tokens[0].Value.ShouldBe("f");
            result.Tokens[1].TokenKind.ShouldBe(CommandTreeToken.Kind.String);
            result.Tokens[1].Value.ShouldBe("bar");
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
            result.Tokens.Count.ShouldBe(2);
            result.Tokens[0].TokenKind.ShouldBe(CommandTreeToken.Kind.LongOption);
            result.Tokens[0].Value.ShouldBe("foo");
            result.Tokens[1].TokenKind.ShouldBe(CommandTreeToken.Kind.String);
            result.Tokens[1].Value.ShouldBe("bar");
        }

        [Fact]
        public void Should_Parse_Remaining_Parameters()
        {
            // Given, When
            var result = CommandTreeTokenizer.Tokenize(
                new[] { "--foo", "--", "--bar", "-qux", "\"lol\"", "w00t" });

            // Then
            result.Tokens.Count.ShouldBe(8);

            result.Remaining.Count.ShouldBe(4);
            result.Remaining[0].ShouldBe("--bar");
            result.Remaining[1].ShouldBe("-qux");
            result.Remaining[2].ShouldBe("\"lol\"");
            result.Remaining[3].ShouldBe("w00t");

            result.Tokens[0].TokenKind.ShouldBe(CommandTreeToken.Kind.LongOption);
            result.Tokens[0].Value.ShouldBe("foo");
            result.Tokens[1].TokenKind.ShouldBe(CommandTreeToken.Kind.Remaining);
            result.Tokens[1].Value.ShouldBe("--");
            result.Tokens[2].TokenKind.ShouldBe(CommandTreeToken.Kind.LongOption);
            result.Tokens[2].Value.ShouldBe("bar");
            result.Tokens[3].TokenKind.ShouldBe(CommandTreeToken.Kind.ShortOption);
            result.Tokens[3].Value.ShouldBe("q");
            result.Tokens[4].TokenKind.ShouldBe(CommandTreeToken.Kind.ShortOption);
            result.Tokens[4].Value.ShouldBe("u");
            result.Tokens[5].TokenKind.ShouldBe(CommandTreeToken.Kind.ShortOption);
            result.Tokens[5].Value.ShouldBe("x");
            result.Tokens[6].TokenKind.ShouldBe(CommandTreeToken.Kind.String);
            result.Tokens[6].Value.ShouldBe("lol");
            result.Tokens[7].TokenKind.ShouldBe(CommandTreeToken.Kind.String);
            result.Tokens[7].Value.ShouldBe("w00t");
        }
    }
}
