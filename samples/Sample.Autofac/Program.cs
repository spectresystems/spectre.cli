using Autofac;
using Sample.Autofac.Commands;
using Spectre.CommandLine;
using Spectre.CommandLine.Autofac;

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
                        foo.AddCommand<BazCommand>("baz");
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

            builder.RegisterType<BarCommand>();
            builder.RegisterType<BarCommand.Settings>();
            builder.RegisterType<BazCommand>();
            builder.RegisterType<BazCommand.Settings>();

            return builder.Build();
        }
    }
}