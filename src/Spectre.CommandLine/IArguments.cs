using System.Linq;

namespace Spectre.CommandLine
{
    public interface IArguments : ILookup<string, string>
    {
    }
}
