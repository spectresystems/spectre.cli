namespace Spectre.CommandLine.Tests.Data
{
    public class BazCommand : Command<BazSettings>
    {
        private readonly CallRecorder _recorder;

        public BazCommand(CallRecorder recorder)
        {
            _recorder = recorder;
        }

        public override int Run(BazSettings settings)
        {
            _recorder.RecordCall($"FooBaz Alpha={settings.Alpha} Beta={settings.Beta} Foo={settings.Foo} Baz={settings.Baz}");
            return 0;
        }
    }
}