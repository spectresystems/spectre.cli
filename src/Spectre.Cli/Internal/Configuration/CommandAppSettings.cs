using System;
using Spectre.Cli.Internal;

namespace Spectre.Cli
{
    internal sealed class CommandAppSettings : ICommandAppSettings
    {
        public string? ApplicationName { get; set; }
        public IConsoleWriter? Console { get; set; }
        public bool PropagateExceptions { get; set; }
        public bool ValidateExamples { get; set; }
        public bool StrictParsing { get; set; }
        public bool XmlDocEnabled { get; set; }
        public bool DebugEnabled { get; set; }

        public ParsingMode ParsingMode =>
            StrictParsing ? ParsingMode.Strict : ParsingMode.Relaxed;

        public bool IsTrue(Func<CommandAppSettings, bool> func, string environmentVariableName)
        {
            if (func(this))
            {
                return true;
            }

            var environmentVariable = Environment.GetEnvironmentVariable(environmentVariableName);
            if (!string.IsNullOrWhiteSpace(environmentVariable))
            {
                if (environmentVariable.Equals("True", StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }
    }
}