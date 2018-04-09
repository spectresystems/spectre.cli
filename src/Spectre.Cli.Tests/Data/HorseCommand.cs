using System.ComponentModel;
using System.Linq;
using Spectre.Cli.Tests.Data.Settings;

namespace Spectre.Cli.Tests.Data
{
    [Description("The horse command.")]
    public class HorseCommand : AnimalCommand<MammalSettings>
    {
        public override int Execute(MammalSettings settings, ILookup<string, string> remaining)
        {
            DumpSettings(settings, remaining);
            return 0;
        }
    }
}
