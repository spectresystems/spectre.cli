using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Spectre.CommandLine.Internal.Parsing.Tokenization
{
    internal sealed class CommandTreeTokenStream : IReadOnlyList<CommandTreeToken>
    {
        private readonly List<CommandTreeToken> _tokens;
        private int _position;

        public int Count => _tokens.Count;

        public CommandTreeToken this[int index] => _tokens[index];

        public CommandTreeToken Current
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

        public CommandTreeTokenStream(IEnumerable<CommandTreeToken> tokens)
        {
            _tokens = new List<CommandTreeToken>(tokens ?? Enumerable.Empty<CommandTreeToken>());
            _position = 0;
        }

        public CommandTreeToken Peek()
        {
            return Peek(0);
        }

        public CommandTreeToken Peek(int index)
        {
            var position = _position + index;
            if (position >= Count)
            {
                return null;
            }
            return _tokens[position];
        }

        public CommandTreeToken Consume(CommandTreeToken.Kind type)
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

        public CommandTreeToken Expect(CommandTreeToken.Kind tokenKind)
        {
            return Expect(new[] { tokenKind });
        }

        public CommandTreeToken Expect(params CommandTreeToken.Kind[] tokenKind)
        {
            if (Current == null)
            {
                var message = $"Expected to find token of type '{tokenKind}' but found null instead.";
                throw new CommandAppException(message);
            }
            if (Current == null || !tokenKind.Contains(Current.TokenKind))
            {
                var message = $"Expected to find token of type '{tokenKind}' but found '{Current.TokenKind}' instead.";
                throw new CommandAppException(message);
            }
            return Current;
        }

        public IEnumerator<CommandTreeToken> GetEnumerator()
        {
            return _tokens.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
