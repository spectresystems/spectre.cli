#tool nuget:?package=GitVersion.CommandLine&version=3.6.2
#load "./buildserver.cake"

public class BuildVersion
{
    public string Version { get; private set; }
    public string SemVersion { get; private set; }
    public string Milestone { get; private set; }

    public static BuildVersion Calculate(ICakeContext context, BuildServer server)
    {
        if (context == null)
        {
            throw new ArgumentNullException("context");
        }

        var version = context.Argument<string>("ver", null);
        var semVersion = version;
        var milestone = $"v{version}";
        
        if (version != null) 
        {
            return new BuildVersion
            {
                Version = version,
                SemVersion = semVersion,
                Milestone = milestone
            };
        }

        if (context.IsRunningOnWindows())
        {
            context.Information("Calculating semantic version...");

            if (server.IsRunningOnAppVeyor)
            {
                // Update AppVeyor version number.
                context.GitVersion(new GitVersionSettings{
                    OutputType = GitVersionOutput.BuildServer,
                });
            }
        }

        var assertedVersions = context.GitVersion(new GitVersionSettings
        {
            OutputType = GitVersionOutput.Json,
        });

        version = assertedVersions.MajorMinorPatch;
        semVersion = assertedVersions.LegacySemVerPadded;
        milestone = string.Concat("v", version);

        if (string.IsNullOrEmpty(version) || string.IsNullOrEmpty(semVersion))
        {
            throw new InvalidOperationException("Could not calculate version.");
        }

        return new BuildVersion
        {
            Version = version,
            SemVersion = semVersion,
            Milestone = milestone
        };
    }
}