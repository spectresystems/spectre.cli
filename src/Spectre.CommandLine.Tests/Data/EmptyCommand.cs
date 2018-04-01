using System.Linq;

namespace Spectre.CommandLine.Tests.Data
{
    public sealed class EmptyCommand : Command<EmptySettings>
    {
        public override int Execute(EmptySettings settings, ILookup<string, string> remaining)
        {
            return 0;
        }
    }

    public sealed class EmptySettings : CommandSettings
    {
    }
}
