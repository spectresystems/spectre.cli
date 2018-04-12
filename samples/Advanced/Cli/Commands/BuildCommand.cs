using System;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Advanced.Utilities;
using Spectre.Cli;

namespace Advanced.Cli.Commands
{
    [Description("Builds a project and all of its dependencies.")]
    public sealed class BuildCommand : AsyncCommand<BuildSettings>
    {
        private readonly IFileSystem _fileSystem;
        private readonly IClock _clock;
        private readonly Random _randomizer;

        public BuildCommand(IFileSystem fileSystem, IClock clock)
        {
            _fileSystem = fileSystem;
            _clock = clock;
            _randomizer = new Random(DateTime.Now.Millisecond);
        }

        // For validation that requires more logic and/or dependencies.
        public override ValidationResult Validate(BuildSettings settings, ILookup<string, string> remaining)
        {
            if (!string.IsNullOrWhiteSpace(settings.Project))
            {
                if (!_fileSystem.FileExist(settings.Project))
                {
                    return ValidationResult.Error("The specified project do not exist.");
                }
            }
            return base.Validate(settings, remaining);
        }

        public override async Task<int> Execute(BuildSettings settings, ILookup<string, string> remaining)
        {
            if (!settings.NoRestore)
            {
                // Pretend we're restoring packages.
                Console.WriteLine("Restoring packages...");
            }
            
            // Get the project name.
            var project = settings.Project;
            if (string.IsNullOrWhiteSpace(project))
            {
                project = "random.csproj";
            }
            
            // Pretend we're building.
            var start = _clock.Now();
            Console.WriteLine($"Building {project}...");
            await Task.Delay(_randomizer.Next(500, 5000));

            // Output some information to the user.
            var delta = _clock.Now() - start;
            Console.WriteLine($"Build completed in {delta.TotalSeconds} seconds.");

            // Return success.
            return 0;
        }
    }
}