using System.ComponentModel;
using Spectre.CommandLine.Tests.Testing;

namespace Spectre.CommandLine.Tests.Data
{
    public class BarCommand : Command<BarCommand.Settings>
    {
        private readonly CallRecorder _recorder;

        public BarCommand(CallRecorder recorder)
        {
            _recorder = recorder;
        }

        public class Settings : FooSettings
        {
            [Option("-b|--bar")]
            [DefaultValue(3)]
            public int Bar { get; set; }
        }

        public override int Run(Settings settings)
        {
            _recorder.RecordCall($"FooBar Foo={settings.Foo} Bar={settings.Bar}");
            return 0;
        }
    }
}