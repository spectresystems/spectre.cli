using System.Linq;
using Spectre.Cli.Tests.Data.Settings;

namespace Spectre.Cli.Tests.Data
{
    public sealed class EmptyCommand : Command<EmptySettings>
    {
        public override int Execute(EmptySettings settings, ILookup<string, string> remaining)
        {
            return 0;
        }
    }
}
