using System.ComponentModel;
using Spectre.Cli.Tests.Data.Settings;

namespace Spectre.Cli.Tests.Data
{
    [Description("The giraffe command.")]
    public sealed class GiraffeCommand : Command<GiraffeSettings>
    {
        public override int Execute(CommandContext context, GiraffeSettings settings)
        {
            return 0;
        }
    }
}
