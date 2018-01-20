using System.Linq;

namespace Spectre.CommandLine.Tests.Data
{
    public class DogCommand : AnimalCommand<DogSettings>
    {
        public override int Execute(DogSettings settings, ILookup<string, string> remaining)
        {
            DumpSettings(settings, remaining);
            return 0;
        }
    }
}