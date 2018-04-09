using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Spectre.Cli.Internal;
using Spectre.Cli.Internal.Configuration;
using Spectre.Cli.Internal.Rendering;
using Spectre.Cli.Internal.Rendering.Elements;

namespace Spectre.Cli
{
    /// <summary>
    /// The entry point for a command line application.
    /// </summary>
    public sealed class CommandApp
    {
        private readonly Configurator _configurator;
        private readonly CommandExecutor _executor;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandApp"/> class.
        /// </summary>
        /// <param name="registrar">The registrar.</param>
        public CommandApp(ITypeRegistrar registrar = null)
        {
            _configurator = new Configurator(registrar);
            _executor = new CommandExecutor(registrar);
        }

        /// <summary>
        /// Configures the command line application.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public void Configure(Action<IConfigurator> configuration)
        {
            configuration(_configurator);
        }

        /// <summary>
        /// Runs the command line application with specified arguments.
        /// </summary>
        /// <param name="args">The arguments.</param>
        /// <returns>The exit code from the executed command.</returns>
        public int Run(IEnumerable<string> args)
        {
            return RunAsync(args).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Runs the command line application with specified arguments.
        /// </summary>
        /// <param name="args">The arguments.</param>
        /// <returns>The exit code from the executed command.</returns>
        public async Task<int> RunAsync(IEnumerable<string> args)
        {
            try
            {
                return await _executor
                    .Execute(_configurator, args)
                    .ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                // Should we propagate exceptions?
                if (_configurator.ShouldPropagateExceptions)
                {
                    throw;
                }

                // Render the exception.
                ConsoleRenderer.Render(GetRenderableErrorMessage(ex));

                // Should we always propagate when debugging?
                if (Debugger.IsAttached
                    && ex is CommandAppException appException
                    && appException.AlwaysPropagateWhenDebugging)
                {
                    throw;
                }

                return -1;
            }
        }

        private static IRenderable GetRenderableErrorMessage(Exception ex)
        {
            if (ex is CommandAppException renderable && renderable.Pretty != null)
            {
                return renderable.Pretty;
            }

            return new BlockElement()
                .Append(new ColorElement(ConsoleColor.Red, new TextElement("Error: ")))
                .Append(new TextElement(ex.Message));
        }
    }
}
