using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Spectre.Cli.Testing.Data.Settings;

namespace Spectre.Cli.Testing.Data.Commands
{
    [Description("The dog command.")]
    public class DogCommand : AnimalCommand<DogSettings>
    {
        [SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification = "Trust me")]
        public override ValidationResult Validate(CommandContext context, DogSettings settings)
        {
            if (settings.Age > 100 && !context.Remaining.Raw.Contains("zombie"))
            {
                return ValidationResult.Error("Dog is too old...");
            }

            return base.Validate(context, settings);
        }

        public override int Execute(CommandContext context, DogSettings settings)
        {
            DumpSettings(context, settings);
            return 0;
        }
    }
}