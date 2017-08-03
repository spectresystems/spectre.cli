namespace Spectre.CommandLine.Parsing
{
    internal class CommandTreeParserContext
    {
        public int CurrentArgumentPosition { get; private set; }

        public void ResetArgumentPosition()
        {
            CurrentArgumentPosition = 0;
        }

        public void IncreaseArgumentPosition()
        {
            CurrentArgumentPosition += 1;
        }
    }
}