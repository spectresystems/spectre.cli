using Sample.Commands;
using Sample.Commands.Settings;
using Spectre.CommandLine;

namespace Sample
{
    public class Program
    {
        public static int Main(string[] args)
        {
            var app = new CommandApp();

            app.Configure(config =>
            {
                config.SetApplicationName("sample");

                config.AddProxy<FooSettings>("foo", foo =>
                {
                    foo.SetDescription("The foo command.");

                    foo.AddCommand<BarCommand>("bar");
                    foo.AddProxy<BazSettings>("baz", baz =>
                    {
                        baz.SetDescription("The baz command.");
                        baz.AddCommand<QuxCommand>("qux");
                    });
                });
            });

            return app.Run(args);
        }
    }
}


