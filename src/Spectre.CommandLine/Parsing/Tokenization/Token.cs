// Licensed to Spectre Systems AB under one or more agreements.
// Spectre Systems AB licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

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
