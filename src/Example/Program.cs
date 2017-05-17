using Example.Commands.Build;
using Example.Commands.Nuget;
using Spectre.CommandLine;

namespace Example
{
    public class Program
    {
        public static int Main(string[] args)
        {
            using (var app = new CommandApp())
            {
                // Set additional information.
                app.SetTitle("Microsoft .NET Core Shared Framework Host");

                // Register commands.
                app.RegisterCommand<BuildCommand>();
                app.RegisterCommand<NuGetCommand>();

                // Run the application.
                return app.Run(args);
            }
        }
    }
}