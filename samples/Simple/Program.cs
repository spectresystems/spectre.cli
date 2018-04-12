using Simple.Commands;
using Simple.Commands.Settings;
using Spectre.Cli;

namespace Simple
{
    public static class Program
    {
        public static int Main(string[] args)
        {
            var app = new CommandApp();
            app.Configure(config =>
            {
                config.AddCommand<BuildCommand>("build");
                config.AddCommand<AddSettings>("add", add =>
                {
                    add.SetDescription("Add reference to the project.");

                    add.AddCommand<AddPackageCommand>("package");
                    add.AddCommand<AddReferenceCommand>("reference");
                });
            });

            return app.Run(args);
        }
    }
}
