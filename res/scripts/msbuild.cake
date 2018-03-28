#load "./version.cake"

public static class MSBuildHelper
{
    public static DotNetCoreMSBuildSettings CreateSettings(BuildVersion version)
    {
        return new DotNetCoreMSBuildSettings()
            .WithProperty("Version", version.SemVersion)
            .WithProperty("AssemblyVersion", version.Version)
            .WithProperty("FileVersion", version.Version);
    }
}
