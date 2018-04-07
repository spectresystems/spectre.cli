using System.ComponentModel;
using Spectre.CommandLine.Tests.Data.Validators;

namespace Spectre.CommandLine.Tests.Data.Settings
{
    public abstract class AnimalSettings : CommandSettings
    {
        [CommandOption("-a|--alive")]
        [Description("Indicates whether or not the animal is alive.")]
        public bool IsAlive { get; set; }

        [CommandArgument(1, "[LEGS]")]
        [Description("The number of legs.")]
        [EvenNumberValidator("Animals must have an even number of legs.")]
        [PositiveNumberValidator("Number of legs must be greater than 0.")]
        public int Legs { get; set; }
    }
}