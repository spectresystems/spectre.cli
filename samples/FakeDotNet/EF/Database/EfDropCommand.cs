using System.Linq;

namespace FakeDotNet.EF.Database
{
    public sealed class EfDropCommand : EfCommand<EfDropSettings>
    {
        protected override int Execute(EfDropSettings settings, ILookup<string, string> remaining)
        {
            DumpSettings(settings, remaining);
            return 0;
        }
    }
}
