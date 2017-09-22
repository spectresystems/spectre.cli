// Licensed to Spectre Systems AB under one or more agreements.
// Spectre Systems AB licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics;

namespace Spectre.CommandLine
{
    /// <summary>
    /// Represents the base class for a command with settings.
    /// </summary>
    /// <typeparam name="TSettings">The command settings type.</typeparam>
    public abstract class Command<TSettings> : ICommand<TSettings>
    {
        /// <summary>
        /// Executes the command with the specified settings.
        /// </summary>
        /// <param name="settings">The command settings.</param>
        /// <returns>The command exit code.</returns>
        public abstract int Run(TSettings settings);

        int ICommand.Run(object settings)
        {
            Debug.Assert(settings is TSettings, "Command settings are of unexpected type.");
            return Run((TSettings)settings);
        }
    }
}