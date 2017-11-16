using System;
using System.Collections.Generic;
using System.Linq;
using Spectre.CommandLine.Internal.Configuration;
using Spectre.CommandLine.Internal.Modelling;
using Spectre.CommandLine.Internal.Parsing;

namespace Spectre.CommandLine.Internal
{
    internal sealed class CommandExecutor
    {
        private readonly ITypeResolver _resolver;

        public CommandExecutor(ITypeResolver resolver)
        {
            _resolver = new TypeResolverAdapter(resolver);
        }

        public int Execute(IConfiguration configuration, IEnumerable<string> args)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }
            if (configuration.Commands.Count == 0)
            {
                throw new CommandAppException("No commands have been configured.");
            }

            // Create the command model.
            var model = CommandModelBuilder.Build(configuration);

            // Parse and map the model against the arguments.
            var parser = new CommandTreeParser(model, new CommandOptionAttribute("-h|--help"));
            var result = parser.Parse(args);

            // Currently the root?
            if (result.tree == null)
            {
                // Display help.
                HelpWriter.Write(model);
                return 0;
            }

            // Get the command to execute.
            var leaf = result.tree.GetLeafCommand();
            if (leaf.Command.IsProxy || leaf.ShowHelp)
            {
                // Proxy's can't be executed. Show help.
                HelpWriter.Write(model, leaf.Command);
                return 0;
            }

            return Execute(leaf, result.tree, result.remaining);
        }

        private int Execute(CommandTree leaf, CommandTree tree, ILookup<string, string> remaining)
        {
            // Create the command and the settings.
            var settings = leaf.CreateSettings(_resolver);

            // Bind the command tree against the settings.
            CommandBinder.Bind(tree, ref settings);

            // Execute the command.
            var command = leaf.CreateCommand(_resolver);
            return command.Execute(settings, remaining);
        }
    }
}
