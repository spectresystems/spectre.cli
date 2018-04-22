using System;

namespace Spectre.Cli
{
    /// <summary>
    /// Represents a configurator.
    /// </summary>
    public interface IConfigurator
    {
        /// <summary>
        /// Sets the name of the application.
        /// </summary>
        /// <param name="name">The name of the application.</param>
        void SetApplicationName(string name);

        /// <summary>
        /// Tells the command line application to propagate all
        /// exceptions to the user.
        /// </summary>
        void PropagateExceptions();

        /// <summary>
        /// Adds a command to the configuration.
        /// </summary>
        /// <typeparam name="TCommand">The command type.</typeparam>
        /// <param name="name">The name of the command.</param>
        void AddCommand<TCommand>(string name) where TCommand : class, ICommand;

        /// <summary>
        /// Adds a command branch to the configuration.
        /// </summary>
        /// <typeparam name="TSettings">The type of the settings.</typeparam>
        /// <param name="name">The name of the command branch.</param>
        /// <param name="action">The command branch configuration.</param>
        void AddBranch<TSettings>(string name, Action<IConfigurator<TSettings>> action) where TSettings : CommandSettings;
    }
}