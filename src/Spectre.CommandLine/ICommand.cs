using System;

namespace Spectre.CommandLine
{
    public interface ICommand
    {
        Type SettingsType { get; }
        string Name { get; }
        void Configure(ICommandRegistrar registrar);
        int Run(object settings);
    }
}
