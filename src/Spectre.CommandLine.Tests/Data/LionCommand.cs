using System.ComponentModel;
using System.Linq;
using Spectre.CommandLine.Tests.Data.Settings;

namespace Spectre.CommandLine.Tests.Data
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