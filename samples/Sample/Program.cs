using Sample.Shared;
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
                config.AddProxy<FooSettings>("foo", foo =>
                {
                    foo.AddCommand<BarCommand>("bar");
                    foo.AddProxy<BazSettings>("baz", baz =>
                    {
                        baz.AddCommand<QuxCommand>("qux");
                    });
                });
            });
            return app.Run(args);
        }
    }
}


