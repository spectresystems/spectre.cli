using System.Linq;

namespace Spectre.Cli
{
    /// <summary>
    /// Represents a collection of remaining arguments.
    /// </summary>
    public interface IRemainingArguments : ILookup<string, string>
    {
    }
}
