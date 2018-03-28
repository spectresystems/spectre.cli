using System.Linq;

namespace Spectre.CommandLine.Tests.Data
{
    public sealed class InvalidCommand : Command<InvalidSettings>
    {
        public override int Execute(InvalidSettings settings, ILookup<string, string> remaining)
        {
            return 0;
        }
    }
}