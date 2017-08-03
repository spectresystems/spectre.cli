using System;
using System.ComponentModel;
using Spectre.CommandLine;
using Spectre.CommandLine.Annotations;

namespace Sample.Solution
{
    public sealed class AddProjectCommand : Command<AddProjectCommand.Settings>
    {
        public override int Run(Settings settings)
        {
            Console.WriteLine();
            Console.WriteLine("ADD: {0} to {1}", settings.Project, settings.Solution);
            Console.WriteLine();
            return 0;
        }

        public sealed class Settings : SolutionSettings
        {
            [Argument(0, "PRJ_FILE")]
            [Description("Project file to operate on. If not specified, the command will search the current directory for one.")]
            public string Project { get; set; }
        }
    }
}