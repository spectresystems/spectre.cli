using System;

namespace Spectre.Cli
{
    /// <summary>
    /// Represents a configurator for specific settings.
    /// </summary>
    /// <typeparam name="TSettings">The type of the settings.</typeparam>
    public interface IConfigurator<in TSettings>
        where TSettings : CommandSettings
    {
        /// <summary>
        /// Sets the description of the command.
        /// </summary>
        /// <param name="description">The description of the command.</param>
        void SetDescription(string description);

        /// <summary>
        /// Adds a command to the configuration.
        /// </summary>
        /// <typeparam name="TCommand">The command type.</typeparam>
        /// <param name="name">The name of the command.</param>
        void AddCommand<TCommand>(string name) where TCommand : class, ICommandLimiter<TSettings>;

        /// <summary>
        /// Adds a proxy command to the configuration.
        /// </summary>
        /// <typeparam name="TDerivedSettings">The type of the derived settings.</typeparam>
        /// <param name="name">The name of the command.</param>
        /// <param name="action">The command configuration.</param>
        void AddCommand<TDerivedSettings>(string name, Action<IConfigurator<TDerivedSettings>> action) where TDerivedSettings : TSettings;
    }
}
