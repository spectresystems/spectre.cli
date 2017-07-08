using System;
using Spectre.CommandLine.Internal;

namespace Spectre.CommandLine
{
    /// <summary>
    /// The command application.
    /// </summary>
    public sealed class CommandApp
    {
        private readonly IResolver _resolver;
        private readonly Configurator _configurator;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandApp"/> class.
        /// </summary>
        public CommandApp()
            : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandApp"/> class.
        /// </summary>
        /// <param name="resolver">The resolver to be used to instanciate types.</param>
        public CommandApp(IResolver resolver)
        {
            _resolver = new ResolverAdapter(resolver);
            _configurator = new Configurator(_resolver);
        }

        /// <summary>
        /// Adds a configuration to the application.
        /// </summary>
        /// <param name="configuration">The configuration action</param>
        public void Configure(Action<IConfigurator> configuration)
        {
            configuration(_configurator);
        }

        /// <summary>
        /// Executes the application.
        /// </summary>
        /// <param name="args">The arguments.</param>
        /// <returns>The application exit code.</returns>
        public int Run(string[] args)
        {
            // Build the application.
            var builder = new ApplicationBuilder(_resolver);
            var application = builder.Build(_configurator);

            // Run the application.
            return application.Execute(args);
        }
    }
}
