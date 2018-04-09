using System.ComponentModel;
using System.Linq;
using Spectre.Cli.Tests.Data.Settings;

namespace Spectre.Cli.Tests.Data
{
    [Description("The giraffe command.")]
    public sealed class GiraffeCommand : Command<GiraffeSettings>
    {
        public override int Execute(GiraffeSettings settings, ILookup<string, string> remaining)
        {
            return 0;
        }
    }
}
