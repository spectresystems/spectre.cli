using System;
using System.Linq;

namespace Spectre.CommandLine.Tests.Data
{
    public class HorseCommand : Command<MammalSettings>
    {
        public override int Execute(MammalSettings settings, ILookup<string, string> remaining)
        {
            throw new NotImplementedException();
        }
    }
}
