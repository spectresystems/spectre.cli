using Spectre.CommandLine.Tests.Testing;

namespace Spectre.CommandLine.Tests.Data
{
    public class BazCommand : Command<BazCommand.Settings>
    {
        private readonly CallRecorder _recorder;

        public BazCommand(CallRecorder recorder)
        {
            _recorder = recorder;
        }

        public class Settings : FooSettings
        {
            [Option("--baz")]
            public int Baz { get; set; }
        }

        public override int Run(Settings settings)
        {
            _recorder.RecordCall("FooBaz");
            return 0;
        }
    }
}