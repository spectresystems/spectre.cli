// Licensed to Spectre Systems AB under one or more agreements.
// Spectre Systems AB licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Spectre.CommandLine
{
    /// <summary>
    /// Represents a command.
    /// </summary>
    public interface ICommand
    {
        /// <summary>
        /// Executes the command with the specified settings.
        /// </summary>
        /// <param name="settings">The command settings.</param>
        /// <returns>The command's exit code.</returns>
        int Run(object settings);
    }

    /// <summary>
    /// Represents a command.
    /// </summary>
    /// <typeparam name="TSettings">The command settings type.</typeparam>
    public interface ICommand<TSettings> : ICommandLimiter<TSettings>
    {
        /// <summary>
        /// Executes the command with the specified settings.
        /// </summary>
        /// <param name="settings">The command settings.</param>
        /// <returns>The command's exit code.</returns>
        int Run(TSettings settings);
    }
}