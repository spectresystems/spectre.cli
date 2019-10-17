using System;

namespace Spectre.Cli
{
    /// <summary>
    /// Contains extensions for <see cref="IConfigurator"/>
    /// and <see cref="IConfigurator{TSettings}"/>.
    /// </summary>
    public static class ConfiguratorExtensions
    {
        /// <summary>
        /// Adds a command branch.
        /// </summary>
        /// <param name="configurator">The configurator.</param>
        /// <param name="name">The name of the command branch.</param>
        /// <param name="action">The command branch configuration.</param>
        public static void AddBranch(
            this IConfigurator configurator,
            string name,
            Action<IConfigurator<CommandSettings>> action)
        {
            configurator.AddBranch<CommandSettings>(name, action);
        }

        /// <summary>
        /// Adds a command branch.
        /// </summary>
        /// <typeparam name="TSettings">The command setting type.</typeparam>
        /// <param name="configurator">The configurator.</param>
        /// <param name="name">The name of the command branch.</param>
        /// <param name="action">The command branch configuration.</param>
        public static void AddBranch<TSettings>(
            this IConfigurator<TSettings> configurator,
            string name,
            Action<IConfigurator<TSettings>> action)
                where TSettings : CommandSettings
        {
            configurator.AddBranch(name, action);
        }

        /// <summary>
        /// Adds a command without settings that executes a delegate.
        /// </summary>
        /// <param name="configurator">The configurator.</param>
        /// <param name="name">The name of the command.</param>
        /// <param name="func">The delegate to execute as part of command execution.</param>
        /// <returns>A command configurator that can be used to configure the command further.</returns>
        public static IConfigurator AddDelegate(
            this IConfigurator configurator,
            string name,
            Func<CommandContext, int> func)
        {
            configurator.AddDelegate<EmptyCommandSettings>(name, (c, _) => func(c));
            return configurator;
        }

        /// <summary>
        /// Adds a command without settings that executes a delegate.
        /// </summary>
        /// <typeparam name="TSettings">The command setting type.</typeparam>
        /// <param name="configurator">The configurator.</param>
        /// <param name="name">The name of the command.</param>
        /// <param name="func">The delegate to execute as part of command execution.</param>
        /// <returns>A command configurator that can be used to configure the command further.</returns>
        public static IConfigurator<TSettings> AddDelegate<TSettings>(
            this IConfigurator<TSettings> configurator,
            string name,
            Func<CommandContext, int> func)
                where TSettings : CommandSettings
        {
            configurator.AddDelegate<TSettings>(name, (c, _) => func(c));
            return configurator;
        }
    }
}
