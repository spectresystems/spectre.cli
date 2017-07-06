using System;

namespace Spectre.CommandLine
{
    /// <summary>
    /// Represents a type resolver.
    /// </summary>
    public interface IResolver
    {
        /// <summary>
        /// Resolves an instance from the specified type.
        /// </summary>
        /// <param name="type">The type to resolve.</param>
        /// <returns>An instance of the specified type.</returns>
        object Resolve(Type type);
    }
}
