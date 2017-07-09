namespace Spectre.CommandLine.Testing.Data
{
    public class FooSettings
    {
        [Option("-f|--foo")]
        public string Foo { get; set; }
    }
}