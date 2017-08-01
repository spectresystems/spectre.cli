using System.ComponentModel;
using Spectre.CommandLine.Annotations;

namespace Sample.Commands.Settings
{
    public abstract class FooSettings
    {
        [Required]
        [Option("-f|--foo <FOO_VALUE>")]
        [Description("Essential to enable fooing of the bar or baz.")]
        public string Foo { get; set; }
    }
}
