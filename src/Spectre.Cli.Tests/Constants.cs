namespace Spectre.Cli.Tests
{
    public static class Constants
    {
        public static string[] VersionCommand { get; } =
            new[]
            {
                Spectre.Cli.Internal.Constants.Commands.Branch,
                Spectre.Cli.Internal.Constants.Commands.Version,
            };

        public static string[] XmlDocCommand { get; } =
            new[]
            {
                Spectre.Cli.Internal.Constants.Commands.Branch,
                Spectre.Cli.Internal.Constants.Commands.XmlDoc,
            };
    }
}
