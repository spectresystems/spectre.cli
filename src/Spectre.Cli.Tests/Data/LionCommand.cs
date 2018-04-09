using System.ComponentModel;
using System.Linq;
using Spectre.Cli.Tests.Data.Settings;

namespace Spectre.Cli.Tests.Data
{
    [Description("The lion command.")]
    public class LionCommand : AnimalCommand<LionSettings>
    {
        public override int Execute(LionSettings settings, ILookup<string, string> remaining)
        {
            return 0;
        }
    }
}