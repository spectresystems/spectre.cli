using System;
using System.Collections.Generic;

namespace Spectre.CommandLine.Tests.Testing
{
    public sealed class CallRecorder
    {
        private readonly HashSet<string> _recorded;

        public CallRecorder()
        {
            _recorded = new HashSet<string>(StringComparer.Ordinal);
        }

        public void RecordCall(string source)
        {
            _recorded.Add(source);
        }

        public bool WasCalled(string source)
        {
            return _recorded.Contains(source);
        }
    }
}
