using Spectre.CommandLine.Annotations;

namespace Spectre.CommandLine.Configuration
{
    internal interface IConfiguration : ICommandContainer
    {
        string ApplicationName { get; }
        OptionAttribute Help { get; }
    }
}