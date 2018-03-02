using System.Linq;
using System.Threading.Tasks;

namespace Spectre.CommandLine
{
    public interface ICommand
    {
        Task<int> Execute(object settings, ILookup<string, string> remaining);
    }
}