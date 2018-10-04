#load "versioning.cake"
#load "buildserver.cake"
#load "github.cake"

public class BuildData
{
    public string Configuration { get; set; }
    public GitHubCredentials Credentials { get; set; }
    public BuildVersion Versioning { get; set; }
    public BuildServer Server { get; set; }
}