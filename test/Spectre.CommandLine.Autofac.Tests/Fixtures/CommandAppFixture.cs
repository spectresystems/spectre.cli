using Autofac;
using Spectre.CommandLine.Testing;
using Spectre.CommandLine.Testing.Data;

namespace Spectre.CommandLine.Autofac.Tests.Fixtures
{
    public sealed class CommandAppFixture
    {
        public CallRecorder CallRecorder { get; set; }

        public CommandAppFixture()
        {
            CallRecorder = new CallRecorder();
        }

        public int Run(string[] args)
        {
            var container = BuildContainer();

            var app = new CommandApp(new AutofacResolver(container));
            app.Configure(config =>
            {
                config.AddProxy<FooSettings>("foo", foo =>
                {
                    foo.AddCommand<BarCommand>("bar");
                });
            });

            return app.Run(args);
        }

        private IContainer BuildContainer()
        {
            var builder = new ContainerBuilder();
            builder.RegisterInstance(CallRecorder);
            builder.RegisterType<BarCommand>();
            return builder.Build();
        }
    }
}
