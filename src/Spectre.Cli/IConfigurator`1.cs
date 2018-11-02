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
        /// Sets the description of the branch.
        /// </summary>
        /// <param name="description">The description of the branch.</param>
        void SetDescription(string description);

        /// <summary>
        /// Adds an example of how to use the branch.
        /// </summary>
        /// <param name="args">The example arguments.</param>
        void AddExample(string[] args);

        /// <summary>
        /// Adds a command.
        /// </summary>
        /// <typeparam name="TCommand">The command type.</typeparam>
        /// <param name="name">The name of the command.</param>
        /// <returns>A command configurator that can be used to configure the command further.</returns>
        ICommandConfigurator AddCommand<TCommand>(string name) where TCommand : class, ICommandLimiter<TSettings>;

        /// <summary>
        /// Adds a command branch.
        /// </summary>
        /// <typeparam name="TDerivedSettings">The type of the derived settings.</typeparam>
        /// <param name="name">The name of the command branch.</param>
        /// <param name="action">The command branch configuration.</param>
        void AddBranch<TDerivedSettings>(string name, Action<IConfigurator<TDerivedSettings>> action) where TDerivedSettings : TSettings;
    }
}
