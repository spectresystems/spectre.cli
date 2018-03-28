using System;
using System.IO;

namespace Spectre.CommandLine.Internal.Parsing.Tokenization
{
    internal sealed class TextBuffer
    {
        private readonly StringReader _reader;

        public bool ReachedEnd => _reader.Peek() == -1;

        public TextBuffer(string text)
        {
            _reader = new StringReader(text);
        }

        public char Peek()
        {
            return (char)_reader.Peek();
        }

        public bool TryPeek(out char character)
        {
            var value = _reader.Peek();
            if (value == -1)
            {
                character = '\0';
                return false;
            }
            character = (char)value;
            return true;
        }

        public void Consume(char character)
        {
            EnsureNotAtEnd();
            if (Read() != character)
            {
                throw new InvalidOperationException($"Expected '{character}' token.");
            }
        }

        public bool IsNext(char character)
        {
            if (TryPeek(out var result))
            {
                return result == character;
            }
            return false;
        }

        public char Read()
        {
            EnsureNotAtEnd();
            return (char)_reader.Read();
        }

        private void EnsureNotAtEnd()
        {
            if (ReachedEnd)
            {
                throw new InvalidOperationException("Can't read past the end of the buffer.");
            }
        }
    }
}
