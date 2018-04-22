using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Spectre.Cli.Internal
{
    internal sealed class RemainingArguments : IRemainingArguments
    {
        private readonly ILookup<string, string> _arguments;

        public int Count => _arguments.Count;
        public IEnumerable<string> this[string key] => _arguments[key];

        public RemainingArguments(ILookup<string, string> arguments)
        {
            _arguments = arguments;
        }

        public IEnumerator<IGrouping<string, string>> GetEnumerator()
        {
            return _arguments.GetEnumerator();
        }

        public bool Contains(string key)
        {
            return _arguments.Contains(key);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
