namespace Spectre.CommandLine.Parsing.Tokenization
{
    internal sealed class Token
    {
        public Type TokenType { get; }
        public string Value { get; }

        public enum Type
        {
            String,
            LongOption,
            ShortOption
        }

        public Token(Type type, string value)
        {
            TokenType = type;
            Value = value;
        }
    }
}
