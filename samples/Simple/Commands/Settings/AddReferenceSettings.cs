using System.ComponentModel;
using Spectre.Cli;

namespace Simple.Commands.Settings
{
    public sealed class AddReferenceSettings : AddSettings
    {
        [CommandArgument(0, "<PROJECT_REFERENCE>")]
        [Description("Project-to-project (P2P) references to add.")]
        public string ProjectReference { get; set; }
    }
}