using System;
using System.Linq;

namespace Spectre.Cli.Tests.Data
{
    public sealed class ThrowingCommand : Command<ThrowingCommandSettings>
    {
        public override int Execute(ThrowingCommandSettings settings, ILookup<string, string> remaining)
        {
            throw new InvalidOperationException("W00t?");
        }
    }

    public sealed class ThrowingCommandSettings : CommandSettings
    {
    }
}
