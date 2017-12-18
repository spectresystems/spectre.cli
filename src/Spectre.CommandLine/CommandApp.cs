using System;
using System.Collections.Generic;
using Spectre.CommandLine.Internal;
using Spectre.CommandLine.Internal.Configuration;

namespace Spectre.CommandLine
{
    public class CommandApp
    {
        private readonly Configurator _configurator;
        private readonly CommandExecutor _executor;

        public CommandApp(ITypeRegistrar registrar = null)
        {
            _configurator = new Configurator(registrar);
            _executor = new CommandExecutor();
        }

        public void Configure(Action<IConfigurator> configuration)
        {
            configuration(_configurator);
        }

        public int Run(IEnumerable<string> args, ITypeResolver resolver = null)
        {
            try
            {
                // Execute the command.
                resolver = resolver ?? new TypeResolverAdapter(null);
                return _executor.Execute(_configurator, args, resolver);
            }
            catch (CommandAppException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return -1;
            }
        }
    }
}
