using System.Linq;
using Spectre.Cli.Tests.Data.Settings;

namespace Spectre.Cli.Tests.Data
{
    public sealed class InvalidCommand : Command<InvalidSettings>
    {
        public override int Execute(InvalidSettings settings, ILookup<string, string> remaining)
        {
            return 0;
        }
    }
}