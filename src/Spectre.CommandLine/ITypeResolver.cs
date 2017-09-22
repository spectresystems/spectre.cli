// Licensed to Spectre Systems AB under one or more agreements.
// Spectre Systems AB licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace Spectre.CommandLine
{
    /// <summary>
    /// Represents a type resolver.
    /// </summary>
    public interface ITypeResolver
    {
        /// <summary>
        /// Resolves an instance from the specified type.
        /// </summary>
        /// <param name="type">The type to resolve.</param>
        /// <returns>An instance of the specified type.</returns>
        object Resolve(Type type);
    }
}
