using System;
using Spectre.Cli.Testing.Fakes;

namespace Spectre.Cli.Testing
{
    internal sealed class CommandAppFixture
    {
        private Action<CommandApp> _appConfiguration = _ => { };
        private Action<IConfigurator> _configuration;

        public void WithDefaultCommand<T>()
            where T : class, ICommand
        {
            _appConfiguration = (app) => app.SetDefaultCommand<T>();
        }

        public void Configure(Action<IConfigurator> action)
        {
            _configuration = action;
        }

        public (int exitCode, string output) Run(params string[] args)
        {
            var writer = new FakeConsoleWriter();

            var app = new CommandApp();
            _appConfiguration?.Invoke(app);

            app.Configure(_configuration);
            app.Configure(c => c.SetOut(writer));
            var result = app.Run(args);

            return (result, writer.ToString());
        }
    }
}
