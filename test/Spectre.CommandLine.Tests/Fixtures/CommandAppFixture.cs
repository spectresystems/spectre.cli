using Spectre.CommandLine.Testing;
using Spectre.CommandLine.Testing.Data;

namespace Spectre.CommandLine.Tests.Fixtures
{
    public sealed class CommandAppFixture
    {
        public CallRecorder CallRecorder { get; set; }
        public TestResolver Resolver { get; set; }

        public CommandAppFixture()
        {
            CallRecorder = new CallRecorder();

            Resolver = new TestResolver();
            Resolver.Register(new BarCommand(CallRecorder));
            Resolver.Register(new BarCommand.Settings());
        }

        public int Run(string[] args)
        {
            var app = new CommandApp(Resolver);
            app.Configure(config =>
            {
                config.AddProxy<FooSettings>("foo", foo =>
                {
                    foo.AddCommand<BarCommand>("bar");
                });
            });
            return app.Run(args);
        }
    }
}
