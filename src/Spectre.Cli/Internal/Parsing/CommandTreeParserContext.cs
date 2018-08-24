using System;
using System.Collections.Generic;
using System.Linq;

namespace Spectre.Cli.Internal.Parsing
{
    internal class CommandTreeParserContext
    {
        private readonly List<string> _args;
        private readonly Dictionary<string, List<string>> _remaining;

        public IReadOnlyList<string> Arguments => _args;
        public int CurrentArgumentPosition { get; private set; }
        public CommandTreeParser.Mode Mode { get; set; }

        public CommandTreeParserContext(IEnumerable<string> args)
        {
            _args = new List<string>(args);
            _remaining = new Dictionary<string, List<string>>(StringComparer.Ordinal);
        }

        public void ResetArgumentPosition()
        {
            CurrentArgumentPosition = 0;
        }

        public void IncreaseArgumentPosition()
        {
            CurrentArgumentPosition++;
        }

        public void AddRemainingArgument(string key, string value)
        {
            if (Mode == CommandTreeParser.Mode.Remaining)
            {
                if (!_remaining.ContainsKey(key))
                {
                    _remaining.Add(key, new List<string>());
                }
                _remaining[key].Add(value);
            }
        }

        public ILookup<string, string> GetRemainingArguments()
        {
            return _remaining
                .SelectMany(pair => pair.Value, (pair, value) => new { pair.Key, value })
                .ToLookup(pair => pair.Key, pair => pair.value);
        }
    }
}