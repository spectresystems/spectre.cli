using System;
using System.Linq;

namespace Spectre.CommandLine.Tests.Data
{
    public class CatCommand : Command<CatSettings>
    {
        public override int Execute(CatSettings settings, ILookup<string, string> remaining)
        {
            throw new NotImplementedException();
        }
    }
}
