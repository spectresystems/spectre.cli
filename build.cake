#tool "nuget:?package=NuGet.CommandLine&version=5.5.1"

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");

// Get the version argument
// TODO: Add support for minver in Cake at some point
var semanticVersion = Argument("buildversion", "0.0.1");
var version = semanticVersion.Split(new char[] { '-' }).FirstOrDefault() ?? semanticVersion;

////////////////////////////////////////////////////////////////
// Tasks

Task("Clean")
    .Does(context => 
{
    CleanDirectory("./.artifacts");
});

Task("Restore")
    .IsDependentOn("Clean")
    .Does(context => 
{
    DotNetCoreRestore("./src/Spectre.Cli.sln");
});

Task("Build")
    .IsDependentOn("Restore")
    .Does(context => 
{
    DotNetCoreBuild("./src/Spectre.Cli.sln", new DotNetCoreBuildSettings {
        Configuration = configuration,
        NoRestore = true,
        NoIncremental = true,
        MSBuildSettings = new DotNetCoreMSBuildSettings()
            .TreatAllWarningsAs(MSBuildTreatAllWarningsAs.Error)
            .WithProperty("Version", version)
            .WithProperty("AssemblyVersion", version)
            .WithProperty("FileVersion", version)
    });
});

Task("Test")
    .IsDependentOn("Build")
    .Does(context => 
{
    DotNetCoreTest("./src/Spectre.Cli.Tests/Spectre.Cli.Tests.csproj", new DotNetCoreTestSettings {
        Configuration = configuration,
        NoRestore = true,
        NoBuild = true,
    });
});

Task("Package")
    .IsDependentOn("Test")
    .Does(context => 
{
    context.DotNetCorePack($"./src/Spectre.Cli/Spectre.Cli.csproj", new DotNetCorePackSettings {
        Configuration = configuration,
        NoRestore = true,
        NoBuild = true,
        OutputDirectory = "./.artifacts",
        MSBuildSettings = new DotNetCoreMSBuildSettings()
            .TreatAllWarningsAs(MSBuildTreatAllWarningsAs.Error)
            .WithProperty("PackageVersion", semanticVersion)
    });
});

Task("Publish-NuGet")
    .IsDependentOn("Package")
    .Does(context => 
{
    // Make sure that there is an API key.
    var apiKey =  context.EnvironmentVariable("NUGET_API_KEY");
    if (string.IsNullOrWhiteSpace(apiKey)) {
        throw new CakeException("No NuGet API key specified.");
    }
    
    var file = File($"./.artifacts/Spectre.Cli.{semanticVersion}.nupkg");
    if (!FileExists(file)) {
        throw new CakeException($"The file {file.Path.FullPath} do not exist.");
    }

    context.Information("Publishing {0}...", file.Path.GetFilename().FullPath);
    context.NuGetPush(file, new NuGetPushSettings {
        ApiKey = apiKey,
        Source = "https://api.nuget.org/v3/index.json"
    });
});

////////////////////////////////////////////////////////////////
// Targets

Task("Publish")
    .IsDependentOn("Publish-NuGet");

Task("Default")
    .IsDependentOn("Package");

////////////////////////////////////////////////////////////////
// Execution

RunTarget(target)