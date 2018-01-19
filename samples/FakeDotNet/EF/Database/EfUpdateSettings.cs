using Spectre.CommandLine;

namespace FakeDotNet.EF.Database
{
    public sealed class EfUpdateSettings : EfCommandSettings
    {
        [CommandArgument(0, "<MIGRATION>")]
        public string Migration { get; set; }
    }
}