using System;
using System.Linq;

namespace Spectre.Cli.Testing.Data.Commands
{
    public abstract class AnimalCommand<TSettings> : Command<TSettings>
        where TSettings : CommandSettings
    {
        protected void DumpSettings(CommandContext context, TSettings settings)
        {
            var properties = settings.GetType().GetProperties();
            foreach (var group in properties.GroupBy(x => x.DeclaringType).Reverse())
            {
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(group.Key.FullName);
                Console.ResetColor();

                foreach (var property in group)
                {
                    Console.WriteLine($"  {property.Name} = {property.GetValue(settings)}");
                }
            }

            if (context.Remaining.Raw.Count > 0)
            {
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Remaining:");
                Console.ResetColor();
                Console.WriteLine(string.Join(", ", context.Remaining));
            }

            Console.WriteLine();
        }
    }
}
