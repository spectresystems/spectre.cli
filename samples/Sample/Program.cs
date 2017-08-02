using Sample.NuGet;
using Sample.Solution;
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
                config.SetApplicationName("dotnet");

                config.AddProxy<NuGetSettings>("nuget", nuget =>
                {
                    nuget.AddCommand<DeletePackageCommand>("delete");
                });

                config.AddProxy<SolutionSettings>("sln", sln =>
                {
                    sln.AddCommand<AddProjectCommand>("add");
                    sln.AddCommand<ListProjectsCommand>("list");
                });
            });

            return app.Run(args);
        }
    }
}


