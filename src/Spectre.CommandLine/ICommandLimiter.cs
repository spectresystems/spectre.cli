// Licensed to Spectre Systems AB under one or more agreements.
// Spectre Systems AB licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Spectre.CommandLine
{
    /// <summary>
    /// Represent a type limiter for commands and should
    /// not be used or implemented directly.
    /// </summary>
    /// <typeparam name="TSettings">The settings type.</typeparam>
    // ReSharper disable once UnusedTypeParameter
    public interface ICommandLimiter<out TSettings> : ICommand
    {
    }
}