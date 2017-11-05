using Shouldly;
using Spectre.CommandLine.Internal.Parsing.Tokenization;
using Xunit;

namespace Spectre.CommandLine.Tests.Unit.Internal.Parsing.Tokenization
{
    public sealed class TokenizerTests
    {
        [Theory]
        [InlineData("foo")]
        public void Should_Parse_String_Correctly(params string[] args)
        {
            // Given, When
            var result = Tokenizer.Tokenize(args);

            // Then
            result.Count.ShouldBe(1);
            result[0].TokenType.ShouldBe(Token.Type.String);
            result[0].Value.ShouldBe("foo");
        }

        [Theory]
        [InlineData("\"foo\"")]
        public void Should_Parse_Quoted_String_Correctly(params string[] args)
        {
            // Given, When
            var result = Tokenizer.Tokenize(args);

            // Then
            result.Count.ShouldBe(1);
            result[0].TokenType.ShouldBe(Token.Type.String);
            result[0].Value.ShouldBe("foo");
        }

        [Theory]
        [InlineData("-f", "bar")]
        public void Should_Parse_Short_Option_Correctly(params string[] args)
        {
            // Given, When
            var result = Tokenizer.Tokenize(args);

            // Then
            result.Count.ShouldBe(2);
            result[0].TokenType.ShouldBe(Token.Type.ShortOption);
            result[0].Value.ShouldBe("f");
            result[1].TokenType.ShouldBe(Token.Type.String);
            result[1].Value.ShouldBe("bar");
        }

        [Theory]
        [InlineData("--foo", "bar")]
        public void Should_Parse_Long_Option_Correctly(params string[] args)
        {
            // Given, When
            var result = Tokenizer.Tokenize(args);

            // Then
            result.Count.ShouldBe(2);
            result[0].TokenType.ShouldBe(Token.Type.LongOption);
            result[0].Value.ShouldBe("foo");
            result[1].TokenType.ShouldBe(Token.Type.String);
            result[1].Value.ShouldBe("bar");
        }
    }
}
