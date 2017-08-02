using System;
using Spectre.CommandLine.Configuration;
using Spectre.CommandLine.Configuration.Parameters;
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
                var configuration = _configurator.Configuration;
                if (configuration.Commands.Count == 0)
                {
                    throw new CommandAppException("No commands have been configured.");
                }

                return RunCore(configuration, args);
            }
            catch (CommandAppException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return -1;
            }
        }

        private int RunCore(IConfiguration configuration, string[] args)
        {
            // Parse the arguments into a command tree.
            var parser = new CommandTreeParser(configuration);
            var tree = parser.Parse(args);
            if (tree == null)
            {
                HelpPrinter.Write(configuration);
                return 0;
            }

            // Get the command to execute.
            var leaf = tree.GetLeafCommand();
            if (leaf.Command.IsProxy || leaf.ShowHelp)
            {
                // Proxys can't be executed. Show help.
                HelpPrinter.Write(leaf.Command, configuration);
                return 0;
            }

            ValidateRequiredParameters(tree);

            var settings = _settingsFactory.CreateSettings(tree, leaf.Command.SettingsType);
            var command = (ICommand) _resolver.Resolve(leaf.Command.CommandType);

            return command.Run(settings);
        }

        private static void ValidateRequiredParameters(CommandTree tree)
        {
            var node = tree.GetRootCommand();
            while (node != null)
            {
                foreach (var parameter in node.Unmapped)
                {
                    if (parameter.Parameter.IsRequired && !parameter.Parameter.IsInherited && !node.ShowHelp)
                    {
                        if (parameter is CommandOption option)
                        {
                            throw new CommandAppException($"Command '{node.Command.Name}' is missing required option '{option.GetOptionName()}'.");
                        }
                        if (parameter is CommandArgument argument)
                        {
                            throw new CommandAppException($"Command '{node.Command.Name}' is missing required argument '{argument.Name}'.");
                        }
                    }
                }
                node = node.Next;
            }
        }
    }
}
