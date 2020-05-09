#load nuget:?package=Spectre.Build&version=0.6.1

///////////////////////////////////////////////////////////////////////////////
// TASKS
///////////////////////////////////////////////////////////////////////////////

Task("Pack-NuGet")
    .PartOf(Spectre.Tasks.Pack)
    .Does<SpectreData>(data =>
{
    DotNetCorePack("./src/Spectre.Cli/Spectre.Cli.csproj", new DotNetCorePackSettings {
        Configuration = data.Configuration,
        OutputDirectory = data.Paths.NuGetPackages,
        NoRestore = true,
        NoBuild = true,
        MSBuildSettings = new DotNetCoreMSBuildSettings()
            .WithProperty("Version", data.Version.SemanticVersion)
            .WithProperty("AssemblyVersion", data.Version.MajorMinorPatchRevision)
            .WithProperty("FileVersion", data.Version.MajorMinorPatchRevision)
            .WithProperty("PackageVersion", data.Version.SemanticVersion)
    });
});

///////////////////////////////////////////////////////////////////////////////
// EXECUTION
///////////////////////////////////////////////////////////////////////////////

Spectre.Build();