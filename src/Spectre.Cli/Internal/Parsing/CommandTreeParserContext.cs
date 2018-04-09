using System.Collections.Generic;
using System.Linq;

namespace Spectre.Cli.Internal.Parsing
{
    internal class CommandTreeParserContext
    {
        private readonly List<string> _args;
        private readonly List<(string name, string value)> _remainingArguments;

        public IReadOnlyList<string> Arguments => _args;
        public int CurrentArgumentPosition { get; private set; }

        public CommandTreeParserContext(IEnumerable<string> args)
        {
            _args = new List<string>(args);
            _remainingArguments = new List<(string, string)>();
        }

        public void ResetArgumentPosition()
        {
            CurrentArgumentPosition = 0;
        }

        public void IncreaseArgumentPosition()
        {
            CurrentArgumentPosition++;
        }

        public void AddRemainingArgument(string name, string value)
        {
            _remainingArguments.Add((name, value));
        }

        public ILookup<string, string> GetRemainingArguments()
        {
            return _remainingArguments.ToLookup(x => x.name, x => x.value);
        }
    }
}