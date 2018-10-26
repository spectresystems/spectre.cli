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
                config.ValidateExamples();

                // Build
                config.AddCommand<BuildCommand>("build")
                    .WithExample(new[] { "build", "--no-restore" });

                // Add
                config.AddBranch<AddSettings>("add", add =>
                {
                    add.SetDescription("Add reference to the project.");

                    // Package
                    add.AddCommand<AddPackageCommand>("package")
                        .WithExample(new[] { "add", "package", "Spectre.Cli" });

                    // Reference
                    add.AddCommand<AddReferenceCommand>("reference")
                        .WithExample(new[] { "add", "reference", "MyProject.csproj" });
                });
            });

            return app.Run(args);
        }
    }
}
