using System;
using System.Collections.Generic;
using Spectre.CommandLine.Internal;
using Spectre.CommandLine.Internal.Configuration;

namespace Spectre.CommandLine
{
    public sealed class CommandApp
    {
        private readonly Configurator _configurator;
        private readonly CommandExecutor _executor;

        public CommandApp()
            : this(null)
        {
        }

        public CommandApp(ITypeResolver resolver)
        {
            _configurator = new Configurator();
            _executor = new CommandExecutor(resolver);
        }

        public void Configure(Action<IConfigurator> configuration)
        {
            configuration(_configurator);
        }

        public int Run(IEnumerable<string> args)
        {
            try
            {
                return _executor.Execute(_configurator, args);
            }
            catch (CommandAppException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return -1;
            }
        }
    }
}
