using System;
using System.ComponentModel;
using Sample.Commands.Settings;
using Spectre.Cli;

namespace Sample.Commands
{
    [Description("Builds a project and all of its dependencies.")]
    public sealed class BuildCommand : Command<BuildSettings>
    {
        public override int Execute(CommandContext context, BuildSettings settings)
        {
            if (settings.EnvironmentVariables != null)
            {
                foreach (var pair in settings.EnvironmentVariables)
                {
                    Console.WriteLine("{0} = '{1}'", pair.Key, pair.Value);
                }
            }

            // Return success.
            return 0;
        }
    }
}