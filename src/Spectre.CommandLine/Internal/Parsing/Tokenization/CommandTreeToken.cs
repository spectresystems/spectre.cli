namespace Spectre.CommandLine.Internal.Parsing.Tokenization
{
    internal sealed class CommandTreeToken
    {
        public Kind TokenKind { get; }
        public string Value { get; }

        public enum Kind
        {
            String,
            LongOption,
            ShortOption
        }

        public CommandTreeToken(Kind kind, string value)
        {
            TokenKind = kind;
            Value = value;
        }
    }
}
