using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using Spectre.Console;

namespace Spectre.Cli.Internal
{
    [Description("Displays the CLI library version")]
    internal sealed class VersionCommand : Command<VersionCommand.Settings>
    {
        private readonly IAnsiConsole _writer;

        public VersionCommand(IConfiguration configuration)
        {
            _writer = configuration.Settings.Console.GetConsole();
        }

        public sealed class Settings : CommandSettings
        {
        }

        public override int Execute(CommandContext context, Settings settings)
        {
            _writer.MarkupLine(
                "[yellow]Spectre.Cli[/] version [aqua]{0}[/]",
                GetVersion(typeof(VersionCommand)?.Assembly));

            _writer.MarkupLine(
                "[yellow]Spectre.Console[/] version [aqua]{0}[/]",
                GetVersion(typeof(IAnsiConsole)?.Assembly));

            return 0;
        }

        private static string GetVersion(Assembly? assembly)
        {
            if (assembly == null)
            {
                return "?";
            }

            try
            {
                var info = FileVersionInfo.GetVersionInfo(assembly.Location);
                return info.ProductVersion ?? "?";
            }
            catch
            {
                return "?";
            }
        }
    }
}
