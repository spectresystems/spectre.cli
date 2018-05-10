using System.ComponentModel;
using Spectre.Cli;

namespace Sample.Commands.Settings
{
    public sealed class AddPackageSettings : AddSettings
    {
        [CommandArgument(0, "<PACKAGE_NAME>")]
        [Description("The package reference to add.")]
        public string PackageName { get; set; }
    }
}