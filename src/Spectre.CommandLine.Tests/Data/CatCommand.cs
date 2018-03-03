using System.Linq;

namespace Spectre.CommandLine.Tests.Data
{
    public class CatCommand : AnimalCommand<CatSettings>
    {
        public override int Execute(CatSettings settings, ILookup<string, string> remaining)
        {
            DumpSettings(settings, remaining);
            return 0;
        }
    }
}
