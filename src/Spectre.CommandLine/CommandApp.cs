using System;
using Spectre.CommandLine.Internal;

namespace Spectre.CommandLine
{
    public sealed class CommandApp
    {
        private readonly IResolver _provider;
        private readonly Configurator _configurator;

        public CommandApp()
            : this(new DefaultResolver())
        {
        }

        public CommandApp(IResolver resolver)
        {
            _provider = resolver ?? throw new ArgumentNullException(nameof(resolver));
            _configurator = new Configurator();
        }

        public void Configure(Action<IConfigurator> configuration)
        {
            configuration(_configurator);
        }

        public int Run(string[] args)
        {
            // Build the application.
            var builder = new ApplicationBuilder(_provider);
            var application = builder.Build(_configurator);

            // Run the application.
            return application.Execute(args);
        }
    }
}
