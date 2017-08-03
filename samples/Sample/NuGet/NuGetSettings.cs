using System.ComponentModel;
using Spectre.CommandLine.Annotations;

namespace Sample.NuGet
{
    public abstract class NuGetSettings
    {
        [Option("-v|--verbosity <verbosity>")]
        [DefaultValue(NuGetVerbosity.Information)]
        public NuGetVerbosity Verbosity { get; set; }
    }
}
