using System.ComponentModel;
using Spectre.CommandLine.Annotations;

namespace Sample.EF
{
    public abstract class EfSettings
    {
        [Option("-v|--verbose")]
        [Description("Show verbose output.")]
        public bool Verbose { get; set; }
    }
}