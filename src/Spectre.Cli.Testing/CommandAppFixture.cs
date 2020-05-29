using System;
using Spectre.Cli.Testing.Fakes;

namespace Spectre.Cli.Testing
{
    internal sealed class CommandAppFixture
    {
        private Action<CommandApp> _appConfiguration = _ => { };
        private Action<IConfigurator> _configuration;

        public CommandAppFixture()
        {
            _configuration = (_) => { };
        }

        public CommandAppFixture WithDefaultCommand<T>()
            where T : class, ICommand
        {
            _appConfiguration = (app) => app.SetDefaultCommand<T>();
            return this;
        }

        public void Configure(Action<IConfigurator> action)
        {
            _configuration = action;
        }

        public (int ExitCode, string Output, CommandContext Context, CommandSettings Settings) Run(params string[] args)
        {
            CommandContext context = null;
            CommandSettings settings = null;
            var writer = new FakeConsoleWriter();

            var app = new CommandApp();
            _appConfiguration?.Invoke(app);

            app.Configure(_configuration);
            app.Configure(c => c.SetInterceptor(new ActionInterceptor((ctx, s) =>
            {
                context = ctx;
                settings = s;
            })));
            app.Configure(c => c.SetOut(writer));
            var result = app.Run(args);

            return (result, writer.ToString(), context, settings);
        }
    }
}
