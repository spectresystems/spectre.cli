#load "data.cake"
#tool "nuget:https://api.nuget.org/v3/index.json?package=gitreleasemanager&version=0.7.1"

public sealed class GitHubCredentials
{
    public string Username { get; }
    public string Password { get; }

    public GitHubCredentials(ICakeContext context)
    {
        Username = context.Argument<string>("github-username", null);
        Password = context.Argument<string>("github-password", null);
    }
}

public static void CreateGitHubRelease(ICakeContext context, BuildData data)
{
    if (string.IsNullOrWhiteSpace(data.Credentials?.Username) ||
        string.IsNullOrWhiteSpace(data.Credentials?.Password))
    {
        throw new InvalidOperationException("No GitHub credentials has been provided.");
    }

    context.GitReleaseManagerCreate(
        data.Credentials.Username,
        data.Credentials.Password,
        "spectresystems", "spectre.cli", 
        new GitReleaseManagerCreateSettings {
            Milestone = data.Versioning.Milestone,
            Name = data.Versioning.Milestone,
            TargetCommitish = "master"
    });
}
