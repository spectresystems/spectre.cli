using System;
using System.ComponentModel;
using Spectre.CommandLine;
using Spectre.CommandLine.Annotations;

namespace Sample.EF.Commands
{
    public sealed class EfDropCommand : Command<EfDropCommand.Settings>
    {
        public sealed class Settings : EfDatabaseSettings
        {
            [Option("--dry-run")]
            [Description("Show which database would be dropped, but don't drop it.")]
            public bool DryRun { get; set; }

            [Option("-s|--startup-project <PROJECT>")]
            public string StartupProject { get; set; }
        }
        
        public override int Run(Settings settings)
        {
            Console.WriteLine("Dropping database!");
            Console.WriteLine();
            Console.WriteLine($"Verbose:         {settings.Verbose}");
            Console.WriteLine($"No color:        {settings.NoColor}");
            Console.WriteLine($"Startup project: {settings.StartupProject}");
            Console.WriteLine($"Dry run:         {settings.DryRun}");

            return 0;
        }
    }
}