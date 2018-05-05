using System.Collections.Generic;

namespace Spectre.Cli.Internal.Parsing
{
    internal class CommandTreeParserContext
    {
        private readonly List<string> _args;
        private readonly List<string> _remaining;

        public IReadOnlyList<string> Arguments => _args;
        public IReadOnlyList<string> Remaining => _remaining;
        public int CurrentArgumentPosition { get; private set; }

        public CommandTreeParserContext(IEnumerable<string> args)
        {
            _args = new List<string>(args);
            _remaining = new List<string>();
        }

        public void ResetArgumentPosition()
        {
            CurrentArgumentPosition = 0;
        }

        public void IncreaseArgumentPosition()
        {
            CurrentArgumentPosition++;
        }

        public void AddRemainingArgument(string arg)
        {
            _remaining.Add(arg);
        }
    }
}