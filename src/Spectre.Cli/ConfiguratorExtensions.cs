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
        /// Sets the name of the application.
        /// </summary>
        /// <param name="configurator">The configurator.</param>
        /// <param name="name">The name of the application.</param>
        /// <returns>A configurator that can be used to configure the application further.</returns>
        public static IConfigurator SetApplicationName(this IConfigurator configurator, string name)
        {
            configurator.Settings.ApplicationName = name;
            return configurator;
        }

        /// <summary>
        /// Sets the console writer.
        /// </summary>
        /// <param name="configurator">The configurator.</param>
        /// <param name="writer">A <see cref="IConsoleWriter"/> that represents the standard output stream.</param>
        /// <returns>A configurator that can be used to configure the application further.</returns>
        public static IConfigurator SetOut(this IConfigurator configurator, IConsoleWriter writer)
        {
            configurator.Settings.Console = writer;
            return configurator;
        }

        /// <summary>
        /// Sets the parsing mode to strict.
        /// </summary>
        /// <param name="configurator">The configurator.</param>
        /// <returns>A configurator that can be used to configure the application further.</returns>
        public static IConfigurator UseStrictParsing(this IConfigurator configurator)
        {
            configurator.Settings.StrictParsing = true;
            return configurator;
        }

        /// <summary>
        /// Tells the command line application to propagate all
        /// exceptions to the user.
        /// </summary>
        /// <param name="configurator">The configurator.</param>
        /// <returns>A configurator that can be used to configure the application further.</returns>
        public static IConfigurator PropagateExceptions(this IConfigurator configurator)
        {
            configurator.Settings.PropagateExceptions = true;
            return configurator;
        }

        /// <summary>
        /// Tells the command line application to validate all
        /// examples before running the application.
        /// </summary>
        /// <param name="configurator">The configurator.</param>
        /// <returns>A configurator that can be used to configure the application further.</returns>
        public static IConfigurator ValidateExamples(this IConfigurator configurator)
        {
            configurator.Settings.ValidateExamples = true;
            return configurator;
        }

        /// <summary>
        /// Enables XML documentation for the command line application.
        /// </summary>
        /// <param name="configurator">The configurator.</param>
        /// <returns>A configurator that can be used to configure the application further.</returns>
        public static IConfigurator EnableXmlDoc(this IConfigurator configurator)
        {
            configurator.Settings.XmlDocEnabled = true;
            return configurator;
        }

        /// <summary>
        /// Enables debug mode for the command line application.
        /// </summary>
        /// <param name="configurator">The configurator.</param>
        /// <returns>A configurator that can be used to configure the application further.</returns>
        public static IConfigurator EnableDebug(this IConfigurator configurator)
        {
            configurator.Settings.DebugEnabled = true;
            return configurator;
        }

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
            configurator.AddBranch(name, action);
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
