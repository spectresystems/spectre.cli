using System;

namespace Spectre.CommandLine
{
    /// <summary>
    /// Represents a configurator.
    /// </summary>
    public interface IConfigurator
    {
        /// <summary>
        /// Adds a command proxy to the configuration.
        /// </summary>
        /// <typeparam name="TSettings">The settings type.</typeparam>
        /// <param name="name">The name of the command.</param>
        /// <param name="action">The configuration action.</param>
        void AddProxy<TSettings>(string name, Action<IConfigurator<TSettings>> action);

        /// <summary>
        /// Adds a command to the configuration.
        /// </summary>
        /// <typeparam name="TCommand">The command type.</typeparam>
        /// <param name="name">The name of the command.</param>
        void AddCommand<TCommand>(string name) where TCommand : ICommand;
    }

    /// <summary>
    /// Represents a configurator.
    /// </summary>
    /// <typeparam name="TSettings">The settings type.</typeparam>
    public interface IConfigurator<in TSettings>
    {
        /// <summary>
        /// Adds a command proxy to the configuration.
        /// </summary>
        /// <typeparam name="TDerivedSettings">The settings type.</typeparam>
        /// <param name="name">The name of the command.</param>
        /// <param name="action">The configuration action.</param>
        void AddProxy<TDerivedSettings>(string name, Action<IConfigurator<TDerivedSettings>> action) where TDerivedSettings : TSettings;

        /// <summary>
        /// Adds a command to the configuration.
        /// </summary>
        /// <typeparam name="TCommand">The command type.</typeparam>
        /// <param name="name">The name of the command.</param>
        void AddCommand<TCommand>(string name) where TCommand : ICommand, ICommandLimiter<TSettings>;
    }
}