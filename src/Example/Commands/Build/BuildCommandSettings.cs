using System.ComponentModel;
using Spectre.CommandLine;

namespace Example.Commands.Build
{
    public sealed class BuildCommandSettings
    {
        [Argument("<PROJECT>", Order = 0)]
        [Description("The MSBuild project file to build.")]
        public string Project { get; set; }

        [Option("-o | --output <OUTPUT_DIR>")]
        [Description("Output directory in which to place built artifacts.")]
        public string Output { get; set; }

        [Option("-f|--framework <FRAMEWORK>")]
        [Description("Target framework to build for.")]
        public string Framework { get; set; }

        [Option("-c|--configuration <CONFIGURATION>")]
        [Description("Configuration to use for building the project.")]
        public string Configuration { get; set; }

        [Option("--version-suffix <VERSION_SUFFIX>")]
        [Description("Defines the value for the $(VersionSuffix) property in the project.")]
        public string VersionSuffic { get; set; }

        [Option("--no-incremental")]
        [Description("Disables incremental build.")]
        public bool NoIncremental { get; set; }

        [Option("--no-dependencies")]
        [Description("Set this flag to ignore project-to-project references.")]
        public bool NoDependencies { get; set; }
    }
}