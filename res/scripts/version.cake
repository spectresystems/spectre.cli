#load "./appveyor.cake"

public class BuildVersion
{
    public string Version { get; private set; }
    public string SemVersion { get; private set; }
    public string Milestone { get; private set; }

    public static BuildVersion Calculate(ICakeContext context, AppVeyorSettings appVeyor)
    {
        if (context == null)
        {
            throw new ArgumentNullException("context");
        }

        if(appVeyor.IsLocal)
        {
            return new BuildVersion
            {
                Version = "0.0.1",
                SemVersion = "0.0.1-Local"
            };
        }

        string version = null;
        string semVersion = null;

        if (context.IsRunningOnWindows())
        {
            context.Information("Calculating Semantic Version");

            if (!appVeyor.IsLocal)
            {
                // Update AppVeyor version number.
                context.GitVersion(new GitVersionSettings{
                    OutputType = GitVersionOutput.BuildServer
                });
            }
        }

        GitVersion assertedVersions = context.GitVersion(new GitVersionSettings
        {
            OutputType = GitVersionOutput.Json,
        });

        version = assertedVersions.MajorMinorPatch;
        semVersion = assertedVersions.LegacySemVerPadded;

        if (string.IsNullOrEmpty(version) || string.IsNullOrEmpty(semVersion))
        {
            context.Information("Fetching version from solution info...");
            version = ReadSolutionInfoVersion(context);
            semVersion = version;
        }

        return new BuildVersion
        {
            Version = version,
            SemVersion = semVersion
        };
    }

    public static string ReadSolutionInfoVersion(ICakeContext context)
    {
        var solutionInfo = context.ParseAssemblyInfo("./src/SolutionInfo.cs");
        if (!string.IsNullOrEmpty(solutionInfo.AssemblyVersion))
        {
            return solutionInfo.AssemblyVersion;
        }
        throw new CakeException("Could not parse version.");
    }
}