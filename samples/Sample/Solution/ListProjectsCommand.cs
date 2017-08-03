using System;
using Spectre.CommandLine;

namespace Sample.Solution
{
    public sealed class ListProjectsCommand : Command<SolutionSettings>
    {
        public override int Run(SolutionSettings settings)
        {
            Console.WriteLine();
            Console.WriteLine("LIST: {0}", settings.Solution);
            Console.WriteLine();
            return 0;
        }
    }
}