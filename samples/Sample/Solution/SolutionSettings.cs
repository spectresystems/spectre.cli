using System.ComponentModel;
using Spectre.CommandLine.Annotations;

namespace Sample.Solution
{
    public abstract class SolutionSettings
    {
        [Argument(0, "SLN_FILE")]
        [Description("Solution file to operate on. If not specified, the command will search the current directory for one.")]
        public string Solution { get; set; }
    }
}