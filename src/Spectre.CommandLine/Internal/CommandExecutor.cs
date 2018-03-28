using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Spectre.CommandLine.Internal.Configuration;
using Spectre.CommandLine.Internal.Modelling;
using Spectre.CommandLine.Internal.Parsing;

namespace Spectre.CommandLine.Internal
{
    internal sealed class CommandExecutor
    {
        private readonly ITypeRegistrar _registrar;

        public CommandExecutor(ITypeRegistrar registrar)
        {
            _registrar = registrar;
        }

        public Task<int> Execute(IConfiguration configuration, IEnumerable<string> args)
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
            var (tree, remaining) = parser.Parse(args);

            // Currently the root?
            if (tree == null)
            {
                // Display help.
                HelpWriter.Write(model);
                return Task.FromResult(0);
            }

            // Get the command to execute.
            var leaf = tree.GetLeafCommand();
            if (leaf.Command.IsProxy || leaf.ShowHelp)
            {
                // Proxy's can't be executed. Show help.
                HelpWriter.Write(model, leaf.Command);
                return Task.FromResult(leaf.ShowHelp ? 0 : 1);
            }

            // Register the arguments with the container.
            var arguments = new Arguments(remaining);
            _registrar?.RegisterInstance(typeof(IArguments), arguments);

            // Create the resolver.
            var resolver = new TypeResolverAdapter(_registrar?.Build());

            // Execute the command tree.
            return Execute(leaf, tree, remaining, resolver);
        }

        private static Task<int> Execute(CommandTree leaf,
            CommandTree tree,
            ILookup<string, string> remaining,
            ITypeResolver resolver)
        {
            // Create the command and the settings.
            var settings = leaf.CreateSettings(resolver);

            // Bind the command tree against the settings.
            CommandBinder.Bind(tree, ref settings, resolver);

            // Create and validate the command.
            var command = leaf.CreateCommand(resolver);
            var validationResult = command.Validate(settings, remaining);
            if (!validationResult.Successful)
            {
                throw new CommandAppException(validationResult.Message);
            }

            // Execute the command.
            return command.Execute(settings, remaining);
        }
    }
}
