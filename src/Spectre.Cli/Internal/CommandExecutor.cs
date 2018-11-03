using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Spectre.Cli.Internal.Configuration;
using Spectre.Cli.Internal.Exceptions;
using Spectre.Cli.Internal.Modelling;
using Spectre.Cli.Internal.Parsing;
using Spectre.Cli.Internal.Rendering;

namespace Spectre.Cli.Internal
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

            // Create the command model.
            var model = CommandModelBuilder.Build(configuration);

            // Parse and map the model against the arguments.
            var parser = new CommandTreeParser(model);
            var parsedResult = parser.Parse(args);

            // Currently the root?
            if (parsedResult.Tree == null)
            {
                // Display help.
                ConsoleRenderer.Render(HelpWriter.Write(model));
                return Task.FromResult(0);
            }

            // Get the command to execute.
            var leaf = parsedResult.Tree.GetLeafCommand();
            if (leaf.Command.IsBranch || leaf.ShowHelp)
            {
                // Branches can't be executed. Show help.
                ConsoleRenderer.Render(HelpWriter.WriteCommand(model, leaf.Command));
                return Task.FromResult(leaf.ShowHelp ? 0 : 1);
            }

            // Register the arguments with the container.
            _registrar?.RegisterInstance(typeof(IRemainingArguments), parsedResult.Remaining);

            // Create the resolver and the context.
            var resolver = new TypeResolverAdapter(_registrar?.Build());
            var context = new CommandContext(parsedResult.Remaining);

            // Execute the command tree.
            return Execute(leaf, parsedResult.Tree, context, resolver);
        }

        private static Task<int> Execute(
            CommandTree leaf,
            CommandTree tree,
            CommandContext context,
            ITypeResolver resolver)
        {
            // Create the command and the settings.
            var settings = leaf.CreateSettings(resolver);

            // Bind the command tree against the settings.
            CommandBinder.Bind(tree, ref settings, resolver);

            // Create and validate the command.
            var command = leaf.CreateCommand(resolver);
            var validationResult = command.Validate(context, settings);
            if (!validationResult.Successful)
            {
                throw RuntimeException.ValidationFailed(validationResult);
            }

            // Execute the command.
            return command.Execute(context, settings);
        }
    }
}
