using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Spectre.CommandLine.Internal;
using Spectre.CommandLine.Internal.Configuration;

namespace Spectre.CommandLine
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
            catch (Exception ex) when (!(ex is ConfigurationException))
            {
                // Should we propagate exceptions?
                if (_configurator.ShouldPropagateExceptions)
                {
                    throw;
                }

                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("Error: ");
                Console.ResetColor();
                Console.WriteLine(ex.Message);

                return -1;
            }
        }
    }
}
