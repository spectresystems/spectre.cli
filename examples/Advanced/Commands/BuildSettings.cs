using System.ComponentModel;
using Sample.Validation;
using Spectre.Cli;

namespace Sample.Commands
{
    public sealed class BuildSettings : CommandSettings
    {
        [ValidateProjectName] // For validating an argument in isolation.
        [CommandArgument(0, "<PROJECT>")]
        [Description("Specifies the project file.")]
        public string Project { get; set; }

        [CommandOption("-p|--password <VALUE>")]
        [Description("The password to use.")]
        public string Password { get; set; }

        [CommandOption("--no-restore")]
        [Description("Doesn't perform an implicit restore during build.")]
        public bool NoRestore { get; set; }

        [CommandOption("--force-restore")]
        [Description("Forces a restore during build.")]
        public bool ForceRestore { get; set; }

        [CommandOption("--serve [PORT]")]
        [DefaultValue(8080)]
        [Description("Serves the content on the specified port.\nDefaults to port [grey]8080[/]")]
        public FlagValue<int> Serve { get; set; }

        // For validating arguments together.
        public override ValidationResult Validate()
        {
            if (NoRestore && ForceRestore)
            {
                return ValidationResult.Error("Both --no-restore and --force-restore have been specified.");
            }

            return base.Validate();
        }
    }
}