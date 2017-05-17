namespace Spectre.CommandLine
{
    public class CommandAppSettings
    {
        public string Name { get; set; }

        public IResolver Resolver { get; set; }

        public IConsoleStreams Streams { get; set; }
    }
}
