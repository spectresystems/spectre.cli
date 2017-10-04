using System;
using Spectre.CommandLine;
using Spectre.CommandLine.Annotations;

namespace Sample.EF.Commands
{
    public sealed class EfUpdateCommand : Command<EfUpdateCommand.Settings>
    {
        public sealed class Settings : EfDatabaseSettings
        {
            [Option("-s|--startup-project <PROJECT>")]
            public string StartupProject { get; set; }
        }

        public override int Run(Settings settings)
        {
            Console.WriteLine("Updating database!");
            Console.WriteLine();
            Console.WriteLine($"Verbose:         {settings.Verbose}");
            Console.WriteLine($"No color:        {settings.NoColor}");
            Console.WriteLine($"Startup project: {settings.StartupProject}");
            
            return 0;
        }
    }
}