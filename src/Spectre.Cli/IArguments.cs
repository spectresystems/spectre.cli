using System.Linq;

namespace Spectre.Cli
{
    /// <summary>
    /// Represents a collection of arguments.
    /// </summary>
    public interface IArguments : ILookup<string, string>
    {
    }
}
