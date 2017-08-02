using System.Collections.Generic;
using System.Linq;

namespace Spectre.CommandLine.Tests
{
    public sealed class CallRecorder
    {
        private readonly List<string> _recorded;

        public CallRecorder()
        {
            _recorded = new List<string>();
        }

        public void RecordCall(string source)
        {
            _recorded.Add(source);
        }

        public bool WasCalled(string source)
        {
            return _recorded.Contains(source);
        }

        public string LastCalled()
        {
            return _recorded.LastOrDefault();
        }
    }
}
