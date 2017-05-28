using Spectre.CommandLine.Tests.Data;
using Spectre.CommandLine.Tests.Testing;

namespace Spectre.CommandLine.Tests.Unit
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
                    foo.AddCommand<BazCommand>("baz");
                });
            });
            return app.Run(args);
        }
    }
}
