using System.Linq;

namespace FakeDotNet.EF.Database
{
    public sealed class EfUpdateCommand : EfCommand<EfUpdateSettings>
    {
        protected override int Execute(EfUpdateSettings settings, ILookup<string, string> remaining)
        {
            DumpSettings(settings, remaining);
            return 0;
        }
    }
}
