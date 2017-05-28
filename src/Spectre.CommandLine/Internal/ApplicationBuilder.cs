using System;
using Microsoft.Extensions.CommandLineUtils;

namespace Spectre.CommandLine.Internal
{
    internal sealed class ApplicationBuilder
    {
        private readonly IResolver _resolver;

        public ApplicationBuilder(IResolver resolver)
        {
            _resolver = resolver;
        }

        public CommandLineApplication Build(ICommandContainer container)
        {
            var app = new CommandLineApplication();
            app.HelpOption("-h|--help");
            app.OnExecute(() =>
            {
                app.ShowHelp();
                return 0;
            });

            foreach (var command in container.Commands)
            {
                Build(app, command);
            }

            return app;
        }

        private void Build(CommandLineApplication parent, CommandDefinition definition, MappingCollection mappings = null)
        {
            parent.Command(definition.Name, app =>
            {
                app.HelpOption("-h|--help");

                // Create the mappings and assign them to the mapping tree.
                mappings = MappingFactory.CreateMappings(app, mappings, definition);

                // Execution
                if (definition.IsProxy)
                {
                    app.OnExecute(() =>
                    {
                        app.ShowHelp();
                        return 0;
                    });
                }
                else
                {
                    app.OnExecute(() => Run(definition, mappings));
                }

                // Register child commands.
                foreach (var command in definition.Commands)
                {
                    Build(app, command, mappings);
                }
            });
        }

        private int Run(CommandDefinition definition, MappingCollection tree)
        {
            // Create the settings.
            var settings = (object)null;
            if (definition.SettingsType != null)
            {
                settings = _resolver.Resolve(definition.SettingsType);
                if (settings == null)
                {
                    throw new InvalidOperationException($"Could not resolve settings of type '{definition.SettingsType.FullName}'.");
                }

                Mapper.Map(settings, tree);
            }

            // Resolve the command.
            var command = _resolver.Resolve(definition.CommandType) as ICommand;
            if (command == null)
            {
                throw new InvalidOperationException($"Could not resolve command of type '{definition.CommandType.FullName}'.");
            }

            return command.Run(settings);
        }
    }
}
