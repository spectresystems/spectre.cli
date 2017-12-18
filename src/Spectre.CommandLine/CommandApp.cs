using System;
using System.Collections.Generic;
using Spectre.CommandLine.Internal;
using Spectre.CommandLine.Internal.Configuration;

namespace Spectre.CommandLine
{
    public sealed class CommandApp
    {
        private readonly ITypeRegistrar _registrar;
        private readonly Configurator _configurator;
        private readonly CommandExecutor _executor;

        public CommandApp(ITypeRegistrar registrar = null)
        {
            _registrar = registrar;
            _configurator = new Configurator(registrar);
            _executor = new CommandExecutor();
        }

        public void Configure(Action<IConfigurator> configuration)
        {
            configuration(_configurator);
        }

        public int Run(IEnumerable<string> args)
        {
            try
            {
                var resolver = new TypeResolverAdapter(_registrar?.Build());
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
