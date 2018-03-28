using System.Linq;
using System.Threading.Tasks;

namespace Spectre.CommandLine
{
    public interface ICommand
    {
        ValidationResult Validate(object settings, ILookup<string, string> remaining);
        Task<int> Execute(object settings, ILookup<string, string> remaining);
    }
}