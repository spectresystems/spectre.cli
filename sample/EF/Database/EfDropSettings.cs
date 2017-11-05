using System.ComponentModel;
using Spectre.CommandLine;

namespace Sample.EF.Database
{
    public sealed class EfDropSettings : EfCommandSettings
    {
        [CommandOption("-f|--force")]
        [Description("Don't confirm.")]
        public bool Force { get; set; }

        [CommandOption("--dry-run")]
        [Description("Show which database would be dropped, but don't drop it.")]
        public bool DryRun { get; set; }
    }
}