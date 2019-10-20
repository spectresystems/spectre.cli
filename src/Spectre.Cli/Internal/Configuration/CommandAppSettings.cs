using Spectre.Cli.Internal;

namespace Spectre.Cli
{
    internal sealed class CommandAppSettings : ICommandAppSettings
    {
        public string? ApplicationName { get; set; }
        public IConsoleWriter? Console { get; set; }
        public bool PropagateExceptions { get; set; }
        public bool ValidateExamples { get; set; }
        public bool Strict { get; set; }

        public ParsingMode ParsingMode => Strict ? ParsingMode.Strict : ParsingMode.Relaxed;
    }
}