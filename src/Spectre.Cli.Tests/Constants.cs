using System.Diagnostics.CodeAnalysis;

namespace Spectre.Cli.Tests
{
    public static class Constants
    {
        [SuppressMessage("Performance", "CA1819:Properties should not return arrays", Justification = "It's OK")]
        public static string[] VersionCommand { get; } =
            new[]
            {
                Spectre.Cli.Internal.Constants.Commands.Branch,
                Spectre.Cli.Internal.Constants.Commands.Version,
            };

        [SuppressMessage("Performance", "CA1819:Properties should not return arrays", Justification = "It's OK")]
        public static string[] XmlDocCommand { get; } =
            new[]
            {
                Spectre.Cli.Internal.Constants.Commands.Branch,
                Spectre.Cli.Internal.Constants.Commands.XmlDoc,
            };
    }
}
