using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Spectre.CommandLine
{
    public abstract class AsyncCommand<TSettings> : ICommand<TSettings>
        where TSettings : class
    {
        public abstract Task<int> Execute(TSettings settings, ILookup<string, string> remaining);

        Task<int> ICommand.Execute(object settings, ILookup<string, string> remaining)
        {
            Debug.Assert(settings is TSettings, "Command settings is of unexpected type.");
            return Execute((TSettings)settings, remaining);
        }
    }
}