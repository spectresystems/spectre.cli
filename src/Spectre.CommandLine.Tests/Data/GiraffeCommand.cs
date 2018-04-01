using System.ComponentModel;
using System.Linq;

namespace Spectre.CommandLine.Tests.Data
{
    public sealed class GiraffeCommand : Command<GiraffeSettings>
    {
        public override int Execute(GiraffeSettings settings, ILookup<string, string> remaining)
        {
            return 0;
        }
    }

    public sealed class GiraffeSettings : CommandSettings
    {
        [CommandArgument(0, "<LENGTH>")]
        [Description("The option description.")]
        public int Length { get; set; }
    }
}
