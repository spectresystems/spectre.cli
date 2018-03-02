using System.Linq;

namespace Spectre.CommandLine.Tests.Data
{
    public class HorseCommand : AnimalCommand<MammalSettings>
    {
        protected override int Execute(MammalSettings settings, ILookup<string, string> remaining)
        {
            DumpSettings(settings, remaining);
            return 0;
        }
    }
}
