using System.ComponentModel;
using Spectre.CommandLine;

namespace FakeDotNet.EF.Database
{
    public sealed class EfDropSettings : EfCommandSettings
    {
        [CommandOption("-f|--force")]
        [Description("Don't confirm.")]
        public bool Force { get; set; }

        [CommandOption("--dry-run")]
        [Description("Show which database would be dropped, but don't drop it.")]
        public bool DryRun { get; set; }

        public override ValidationResult Validate()
        {
            if (DryRun && Force)
            {
                return ValidationResult.Error("Not sure how to force a dry-run...");
            }
            return ValidationResult.Success();
        }
    }
}