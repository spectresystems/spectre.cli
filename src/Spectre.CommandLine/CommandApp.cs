using System;
using Spectre.CommandLine.Configuration;
using Spectre.CommandLine.Parsing;
using Spectre.CommandLine.Utilities;

namespace Spectre.CommandLine
{
    /// <summary>
    /// The command application.
    /// </summary>
    public sealed class CommandApp
    {
        private readonly ITypeResolver _resolver;
        private readonly Configurator _configurator;
        private readonly CommandSettingsFactory _settingsFactory;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="CommandApp"/> class.
        /// </summary>
        public CommandApp()
            : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandApp"/> class.
        /// </summary>
        /// <param name="resolver">The resolver to be used to instanciate types.</param>
        public CommandApp(ITypeResolver resolver)
        {
            _resolver = new TypeResolverAdapter(resolver);
            _configurator = new Configurator();
            _settingsFactory = new CommandSettingsFactory(_resolver);
        }

        /// <summary>
        /// Adds a configuration to the application.
        /// </summary>
        /// <param name="configuration">The configuration action</param>
        public void Configure(Action<IConfigurator> configuration)
        {
            configuration(_configurator);
        }

        /// <summary>
        /// Executes the application.
        /// </summary>
        /// <param name="args">The arguments.</param>
        /// <returns>The application exit code.</returns>
        public int Run(string[] args)
        {
            try
            {
                return RunCore(args);
            }
            catch (CommandAppException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return -1;
            }
        }

        private int RunCore(string[] args)
        {
            var configuration = _configurator.Configuration;
            if (configuration.Commands.Count == 0)
            {
                return -1;
            }

            // Parse the command tree.
            var parser = new CommandTreeParser(configuration);
            var tree = parser.Parse(args);
            if (tree == null)
            {
                HelpPrinter.Write(configuration);
                return 0;
            }

            // Get the top command node.
            var leaf = tree.GetTopCommand();
            if (leaf.Command.IsProxy || leaf.ShowHelp)
            {
                // Proxys can't be executed. Show help.
                HelpPrinter.Write(leaf.Command, configuration);
                return 0;
            }

            // Check if there's any required parameters not set for command.
            ValidateRequiredParameters(tree);

            // Create a mapped settings object.
            var settings = _settingsFactory.CreateSettings(tree, leaf.Command.SettingsType);

            // Create the command instance and run it with the provided settings.
            var command = (ICommand) _resolver.Resolve(leaf.Command.CommandType);
            return command.Run(settings);
        }

        private static void ValidateRequiredParameters(CommandTree tree)
        {
            var node = tree.GetBottomCommand();
            while (node != null)
            {
                foreach (var parameter in node.Unmapped)
                {
                    if (parameter.Info.IsRequired && !parameter.Info.IsInherited && !node.ShowHelp)
                    {
                        if (parameter is CommandOption option)
                        {
                            throw new CommandAppException(
                                $"Command '{node.Command.Name}' is missing required option '{option.GetOptionName()}'.");
                        }
                    }
                }
                node = node.Next;
            }
        }
    }
}
