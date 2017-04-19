using System;

namespace Spectre.CommandLine
{
    public abstract class Command<TSettings> : ICommand
    {
        Type ICommand.SettingsType => typeof(TSettings);

        public string Name { get; }

        protected int Success = 0;
        protected int Error = 1;

        protected Command(string name)
        {
            Name = name;
        }

        public virtual void Configure(ICommandRegistrar registrar)
        {
        }

        int ICommand.Run(object settings)
        {
            return Run((TSettings)settings);
        }

        public abstract int Run(TSettings settings);
    }
}