using System.Linq;

namespace Spectre.CommandLine
{
    /// <summary>
    /// Represents a collection of arguments.
    /// </summary>
    public interface IArguments : ILookup<string, string>
    {
    }
}
