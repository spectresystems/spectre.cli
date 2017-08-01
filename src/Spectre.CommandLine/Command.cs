using System.Diagnostics;

namespace Spectre.CommandLine
{
    /// <summary>
    /// Represents the base class for a command with settings.
    /// </summary>
    /// <typeparam name="TSettings"></typeparam>
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
            Debug.Assert(settings is TSettings);
            return Run((TSettings)settings);
        }
    }
}