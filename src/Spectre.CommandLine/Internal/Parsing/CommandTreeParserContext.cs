using System.Collections.Generic;
using System.Linq;

namespace Spectre.CommandLine.Internal.Parsing
{
    internal class CommandTreeParserContext
    {
        private readonly List<(string name, string value)> _remainingArguments;

        public int CurrentArgumentPosition { get; private set; }

        public CommandTreeParserContext()
        {
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