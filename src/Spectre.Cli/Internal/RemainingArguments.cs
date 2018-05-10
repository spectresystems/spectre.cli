using System.Collections;
using System.Collections.Generic;

namespace Spectre.Cli.Internal
{
    internal sealed class RemainingArguments : IRemainingArguments
    {
        private readonly IReadOnlyList<string> _remaining;

        public int Count => _remaining.Count;
        public string this[int index] => _remaining[index];

        public RemainingArguments(IReadOnlyList<string> remaining)
        {
            _remaining = remaining;
        }

        public IEnumerator<string> GetEnumerator()
        {
            return _remaining.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
