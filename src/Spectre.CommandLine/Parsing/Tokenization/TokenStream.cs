using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Spectre.CommandLine.Parsing.Tokenization
{
    internal sealed class TokenStream : IReadOnlyList<Token>
    {
        private readonly List<Token> _tokens;
        private int _position;

        public int Count => _tokens.Count;

        public Token this[int index] => _tokens[index];

        public Token Current
        {
            get
            {
                if (_position >= Count)
                {
                    return null;
                }
                return _tokens[_position];
            }
        }

        public TokenStream(IEnumerable<Token> tokens)
        {
            _tokens = new List<Token>(tokens ?? Enumerable.Empty<Token>());
            _position = 0;
        }

        public Token Peek()
        {
            return Peek(0);
        }

        public Token Peek(int index)
        {
            var position = _position + index;
            if (position >= Count)
            {
                return null;
            }
            return _tokens[position];
        }

        public Token Consume(Token.Type type)
        {
            Expect(type);
            if (_position >= Count)
            {
                return null;
            }
            var token = _tokens[_position];
            _position++;
            return token;
        }

        public Token Expect(Token.Type tokenType)
        {
            return Expect(new[] { tokenType });
        }

        public Token Expect(params Token.Type[] tokenType)
        {
            if (Current == null)
            {
                var message = $"Expected to find token of type '{tokenType}' but found null instead.";
                throw new CommandAppException(message);
            }
            if (Current == null || !tokenType.Contains(Current.TokenType))
            {
                var message = $"Expected to find token of type '{tokenType}' but found '{Current.TokenType}' instead.";
                throw new CommandAppException(message);
            }
            return Current;
        }

        public IEnumerator<Token> GetEnumerator()
        {
            return _tokens.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
