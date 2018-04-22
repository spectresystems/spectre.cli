namespace Spectre.Cli.Internal.Parsing
{
    internal sealed class CommandTreeToken
    {
        public Kind TokenKind { get; }
        public int Position { get; }
        public string Value { get; }
        public string Representation { get; }

        public enum Kind
        {
            String,
            LongOption,
            ShortOption
        }

        public CommandTreeToken(Kind kind, int position, string value, string representation)
        {
            TokenKind = kind;
            Position = position;
            Value = value;
            Representation = representation;
        }
    }
}
