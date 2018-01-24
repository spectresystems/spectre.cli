public sealed class AppVeyorSettings
{
    public bool IsLocal { get; set; }
    public bool IsRunningOnAppVeyor { get; set; }
    public bool IsPullRequest { get; set; }
    public bool IsDevelopBranch { get; set; }
    public bool IsMasterBranch { get; set; }
    public bool IsTaggedBuild { get; set; }

    public static AppVeyorSettings Initialize(ICakeContext context)
    {
        var buildSystem = context.BuildSystem();
        var branchName = buildSystem.AppVeyor.Environment.Repository.Branch;

        return new AppVeyorSettings
        {
            IsLocal = buildSystem.IsLocalBuild,
            IsRunningOnAppVeyor = buildSystem.AppVeyor.IsRunningOnAppVeyor,
            IsPullRequest = buildSystem.AppVeyor.Environment.PullRequest.IsPullRequest,
            IsDevelopBranch = "develop".Equals(branchName, StringComparison.OrdinalIgnoreCase),
            IsMasterBranch = "master".Equals(branchName, StringComparison.OrdinalIgnoreCase),
            IsTaggedBuild = IsBuildTagged(buildSystem)
        };
    }

    public static bool IsBuildTagged(BuildSystem buildSystem)
    {
        return buildSystem.AppVeyor.Environment.Repository.Tag.IsTag
            && !string.IsNullOrWhiteSpace(buildSystem.AppVeyor.Environment.Repository.Tag.Name);
    }
}