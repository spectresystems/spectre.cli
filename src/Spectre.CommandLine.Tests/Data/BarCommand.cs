namespace Spectre.CommandLine.Tests.Data
{
    public class BarCommand : Command<BarSettings>
    {
        private readonly CallRecorder _recorder;

        public BarCommand(CallRecorder recorder)
        {
            _recorder = recorder;
        }

        public override int Run(BarSettings settings)
        {
            _recorder.RecordCall($"FooBar Foo={settings.Foo} Bar={settings.Bar} Qux={settings.Qux}");
            return 0;
        }
    }
}