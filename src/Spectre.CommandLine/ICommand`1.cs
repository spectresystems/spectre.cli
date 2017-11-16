using System.Linq;

namespace Spectre.CommandLine
{
    public interface ICommand<TSettings> : ICommandLimiter<TSettings>
        where TSettings : class
    {
        int Execute(TSettings settings, ILookup<string, string> remaining);
    }
}
