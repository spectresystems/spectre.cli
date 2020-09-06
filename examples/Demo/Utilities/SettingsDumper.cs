using Spectre.Cli;
using Spectre.Console;

namespace Demo.Utilities
{
    public static class SettingsDumper
    {
        public static void Dump(CommandSettings settings)
        {
            var table = new Table { BorderKind = BorderKind.Rounded };
            table.AddColumn("[grey]Name[/]");
            table.AddColumn("[grey]Value[/]");

            var properties = settings.GetType().GetProperties();
            foreach (var property in properties)
            {
                var value = property.GetValue(settings)
                    ?.ToString()
                    ?.Replace("[", "[[");

                table.AddRow(
                    property.Name,
                    value ?? "[grey]null[/]");
            }

            AnsiConsole.Render(table);
        }
    }
}
