using System;

namespace Spectre.CommandLine
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
        /// Adds a proxy command to the configuration.
        /// </summary>
        /// <typeparam name="TSettings">The type of the settings.</typeparam>
        /// <param name="name">The name of the command.</param>
        /// <param name="action">The command configuration.</param>
        void AddCommand<TSettings>(string name, Action<IConfigurator<TSettings>> action) where TSettings : CommandSettings;
    }
}