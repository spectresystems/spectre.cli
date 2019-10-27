using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Spectre.Cli.Exceptions;
using Spectre.Cli.Internal.Configuration;
using Spectre.Cli.Internal.Modelling;
using Spectre.Cli.Internal.Parsing;
using Spectre.Cli.Internal.Rendering;

namespace Spectre.Cli.Internal
{
    internal sealed class CommandExecutor
    {
        private readonly ITypeRegistrar? _registrar;

        private enum Switch
        {
            None = 0,
            XmlDocs = 1,
            Debug = 2,
        }

        public CommandExecutor(ITypeRegistrar? registrar)
        {
            _registrar = registrar;
        }

        public Task<int> Execute(IConfiguration configuration, IEnumerable<string> args)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            // TODO: Hack
            var @switch = Switch.None;
            if (args.Count() > 0)
            {
                if (args.ElementAt(0).Equals("__xmldoc", StringComparison.OrdinalIgnoreCase))
                {
                    if (configuration.Settings.IsTrue(c => c.XmlDocEnabled, "SPECTRE_CLI_XMLDOC"))
                    {
                        @switch = Switch.XmlDocs;
                        args = args.Skip(1);
                    }
                }
                else if (args.ElementAt(0).Equals("__debug", StringComparison.OrdinalIgnoreCase))
                {
                    if (configuration.Settings.IsTrue(c => c.DebugEnabled, "SPECTRE_CLI_DEBUG"))
                    {
                        @switch = Switch.Debug;
                        args = args.Skip(1);
                    }
                }
                else if (args.ElementAt(0).Equals("__version", StringComparison.OrdinalIgnoreCase))
                {
                    // Output the Spectre.Cli version.
                    var version = typeof(CommandExecutor)?.Assembly?.GetName()?.Version?.ToString();
                    version = version ?? "?";
                    var writer = configuration.Settings.Console ?? new DefaultConsoleWriter();
                    writer.Write($"Spectre.Cli version {version}");
                    return Task.FromResult(0);
                }
            }

            return Execute(configuration, args, @switch);
        }

        private Task<int> Execute(IConfiguration configuration, IEnumerable<string> args, Switch @switch)
        {
            // Create the command model.
            var model = CommandModelBuilder.Build(configuration);

            // Show XML docs?
            if (@switch == Switch.XmlDocs)
            {
                var xml = CommandModelSerializer.Serialize(model);
                var writer = configuration.Settings.Console ?? new DefaultConsoleWriter();
                writer.Write(xml);
                return Task.FromResult(0);
            }

            // Parse and map the model against the arguments.
            var parser = new CommandTreeParser(model);
            var parsedResult = parser.Parse(args);

            // Debugging?
            if (@switch == Switch.Debug)
            {
                var xml = CommandTreeSerializer.Serialize(parsedResult);
                var writer = configuration.Settings.Console ?? new DefaultConsoleWriter();
                writer.Write(xml);
                return Task.FromResult(0);
            }

            // Currently the root?
            if (parsedResult.Tree == null)
            {
                // Display help.
                ConsoleRenderer.Render(
                    HelpWriter.Write(model),
                    configuration.Settings.Console);
                return Task.FromResult(0);
            }

            // Get the command to execute.
            var leaf = parsedResult.Tree.GetLeafCommand();
            if (leaf.Command.IsBranch || leaf.ShowHelp)
            {
                // Branches can't be executed. Show help.
                ConsoleRenderer.Render(
                    HelpWriter.WriteCommand(model, leaf.Command),
                    configuration.Settings.Console);
                return Task.FromResult(leaf.ShowHelp ? 0 : 1);
            }

            // Register the arguments with the container.
            _registrar?.RegisterInstance(typeof(IRemainingArguments), parsedResult.Remaining);

            // Create the resolver and the context.
            var resolver = new TypeResolverAdapter(_registrar?.Build());
            var context = new CommandContext(parsedResult.Remaining, leaf.Command.Name, leaf.Command.Data);

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
