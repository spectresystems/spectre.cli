using Sample.Commands;
using Sample.Commands.Settings;
using Spectre.Cli;

namespace Sample
{
    public static class Program
    {
        public static int Main(string[] args)
        {
            var app = new CommandApp();
            app.Configure(config =>
            {
                // Build
                config.AddCommand<BuildCommand>("build")
                    .WithExample("build --no-restore");

                // Add
                config.AddBranch<AddSettings>("add", add =>
                {
                    add.SetDescription("Add reference to the project.");

                    // Package
                    add.AddCommand<AddPackageCommand>("package")
                        .WithExample("add package Spectre.Cli");

                    // Reference
                    add.AddCommand<AddReferenceCommand>("reference")
                        .WithExample("add", "reference", "MyProject.csproj");
                });
            });

            return app.Run(args);
        }
    }
}
