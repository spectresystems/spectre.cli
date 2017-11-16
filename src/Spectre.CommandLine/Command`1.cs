using System.Diagnostics;
using System.Linq;

namespace Spectre.CommandLine
{
    public abstract class Command<TSettings> : ICommand<TSettings>
        where TSettings : class
    {
        public abstract int Execute(TSettings settings, ILookup<string, string> remaining);

        int ICommand.Execute(object settings, ILookup<string, string> remaining)
        {
            Debug.Assert(settings is TSettings, "Command settings is of unexpected type.");
            return Execute((TSettings)settings, remaining);
        }
    }
}
