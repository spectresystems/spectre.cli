using System.Linq;
using System.Threading.Tasks;

namespace Spectre.CommandLine
{
    public interface ICommand<TSettings> : ICommandLimiter<TSettings>
        where TSettings : class
    {
        Task<int> Execute(TSettings settings, ILookup<string, string> remaining);
    }
}
