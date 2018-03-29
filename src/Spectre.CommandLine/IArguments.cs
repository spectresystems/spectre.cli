using System.Linq;

namespace Spectre.CommandLine
{
    /// <summary>
    /// Represents a collection of arguments.
    /// </summary>
    /// <inheritdoc />
    public interface IArguments : ILookup<string, string>
    {
    }
}
