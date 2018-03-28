using System.Linq;

namespace FakeDotNet.EF.DbContext
{
    public sealed class EfScaffoldCommand : EfCommand<EfScaffoldSettings>
    {
        public override int Execute(EfScaffoldSettings settings, ILookup<string, string> remaining)
        {
            DumpSettings(settings, remaining);
            return 0;
        }
    }
}
