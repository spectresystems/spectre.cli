using System.Linq;

namespace Spectre.CommandLine.Tests.Data
{
    public class DogCommand : Command<DogSettings>
    {
        public override int Execute(DogSettings settings, ILookup<string, string> remaining)
        {
            return 0;
        }
    }
}