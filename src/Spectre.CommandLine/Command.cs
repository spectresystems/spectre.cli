using System.Diagnostics;

namespace Spectre.CommandLine
{
    public abstract class Command : ICommand
    {
        protected abstract int Run();

        int ICommand.Run(object settings)
        {
            return Run();
        }
    }

    public abstract class Command<TSettings> : ICommand<TSettings>
    {
        public abstract int Run(TSettings settings);

        int ICommand.Run(object settings)
        {
            Debug.Assert(settings is TSettings);
            return Run((TSettings)settings);
        }
    }
}