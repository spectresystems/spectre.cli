using System.ComponentModel;
using Spectre.Cli;

namespace FakeDotNet.EF.Database
{
    public sealed class EfUpdateSettings : EfCommandSettings
    {
        [CommandArgument(0, "<MIGRATION>")]
        [Description("The migration to use when upgrading the database.")]
        public string Migration { get; set; }
    }
}