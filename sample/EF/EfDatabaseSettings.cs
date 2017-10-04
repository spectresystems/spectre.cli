using System.ComponentModel;
using Spectre.CommandLine.Annotations;

namespace Sample.EF
{
    public abstract class EfDatabaseSettings : EfSettings
    {
        [Option("--no-color")]
        [Description("Don't colorize output.")]
        public bool NoColor { get; set; }
    }
}