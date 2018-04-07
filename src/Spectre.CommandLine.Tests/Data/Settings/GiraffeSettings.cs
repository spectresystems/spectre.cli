using System.ComponentModel;

namespace Spectre.CommandLine.Tests.Data.Settings
{
    public sealed class GiraffeSettings : MammalSettings
    {
        [CommandArgument(0, "<LENGTH>")]
        [Description("The option description.")]
        public int Length { get; set; }
    }
}