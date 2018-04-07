using System.Linq;
using Spectre.CommandLine.Tests.Data.Settings;

namespace Spectre.CommandLine.Tests.Data
{
    public sealed class EmptyCommand : Command<EmptySettings>
    {
        public override int Execute(EmptySettings settings, ILookup<string, string> remaining)
        {
            return 0;
        }
    }
}
