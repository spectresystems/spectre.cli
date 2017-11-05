using System.Linq;

namespace Spectre.CommandLine
{
    public interface ICommand
    {
        int Execute(object settings, ILookup<string, string> remaining);
    }
}