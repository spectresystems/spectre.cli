namespace Spectre.CommandLine.Internal.Parsing.Tokenization
{
    internal sealed class TemplateToken
    {
        public Kind TokenKind { get; }
        public string Value { get; }

        public TemplateToken(Kind kind, string value)
        {
            TokenKind = kind;
            Value = value;
        }

        public enum Kind
        {
            LongName,
            ShortName,
            RequiredValue,
            OptionalValue
        }
    }
}