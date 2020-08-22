using System;
using System.Threading.Tasks;
using Autofac;
using Sample.Autofac;
using Sample.Commands;
using Spectre.Cli;

namespace Sample
{
    public static class Program
    {
        public static async Task<int> Main(string[] args)
        {
            var registrar = new AutofacTypeRegistrar(BuildContainer());
            var app = new CommandApp(registrar);

            app.Configure(config =>
            {
                config.SetApplicationName("advanced.exe");
                config.ValidateExamples();

                // You can add the command directly.
                config.AddCommand<BuildCommand>("build")
                    .WithExample(new[] { "build", "test.csproj" });

                // Add a branched command hierarchy.
                config.AddBranch("foo", foo =>
                {
                    // Since we've created a branch without
                    // specifying what settings the branch should
                    // be based on, we can put anything in this level.
                    foo.AddCommand<BuildCommand>("build");

                    // Create a new branch based on BuildSettings.
                    foo.AddBranch<BuildSettings>("bar", bar =>
                    {
                        // We're now forced to use commands that
                        // inherit its settings from BuildSettings.
                        bar.AddCommand<BuildCommand>("build");

                        // Add a delegate command based on BuildSettings.
                        bar.AddDelegate<BuildSettings>("qux", (c, s) =>
                        {
                            Console.WriteLine("Project:", s.Project);
                            return 0; // Exit code
                        })
                        // Add an example to the configured command.
                        .WithDescription("Print project name.");
                    });
                });
            });

            return await app.RunAsync(args);
        }

        private static ContainerBuilder BuildContainer()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<FileSystem>().As<IFileSystem>().SingleInstance();
            return builder;
        }
    }
}
