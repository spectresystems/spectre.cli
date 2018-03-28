using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Spectre.CommandLine.Internal;
using Spectre.CommandLine.Internal.Configuration;

namespace Spectre.CommandLine
{
    public sealed class CommandApp
    {
        private readonly Configurator _configurator;
        private readonly CommandExecutor _executor;

        public CommandApp(ITypeRegistrar registrar = null)
        {
            _configurator = new Configurator(registrar);
            _executor = new CommandExecutor(registrar);
        }

        public void Configure(Action<IConfigurator> configuration)
        {
            configuration(_configurator);
        }

        public int Run(IEnumerable<string> args)
        {
            return RunAsync(args).GetAwaiter().GetResult();
        }

        public async Task<int> RunAsync(IEnumerable<string> args)
        {
            try
            {
                return await _executor
                    .Execute(_configurator, args)
                    .ConfigureAwait(false);
            }
            catch (Exception ex) when (ex.GetType() != typeof(ConfigurationException))
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
