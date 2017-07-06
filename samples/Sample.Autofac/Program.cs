using Autofac;
using Spectre.CommandLine;
using Spectre.CommandLine.Autofac;
using Sample.Shared;

namespace Sample.Autofac
{
    public class Program
    {
        public static int Main(string[] args)
        {
            using (var container = BuildContainer())
            {
                // Create the application.
                var app = new CommandApp(new AutofacResolver(container));

                // Configure the commands.
                app.Configure(config =>
                {
                    config.AddProxy<FooSettings>("foo", foo =>
                    {
                        foo.AddCommand<BarCommand>("bar");
                        foo.AddProxy<BazSettings>("baz", baz =>
                        {
                            baz.AddCommand<QuxCommand>("qux");
                        });
                    });
                });

                // Run the application.
                return app.Run(args);
            }
        }

        private static ILifetimeScope BuildContainer()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<Greeting>();

            // Register top level commands.
            builder.RegisterType<BarCommand>();
            builder.RegisterType<BarCommand.Settings>();
            builder.RegisterType<QuxCommand>();
            builder.RegisterType<QuxCommand.Settings>();

            return builder.Build();
        }
    }
}