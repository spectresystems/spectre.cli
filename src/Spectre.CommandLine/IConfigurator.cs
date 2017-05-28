using System;

namespace Spectre.CommandLine
{
    public interface IConfigurator
    {
        void AddProxy<TSettings>(string name, Action<IConfigurator<TSettings>> action);
        void AddCommand<TCommand>(string name) where TCommand : ICommand;
    }

    public interface IConfigurator<in TSettings>
    {
        void AddProxy<TDerivedSettings>(string name, Action<IConfigurator<TDerivedSettings>> action) where TDerivedSettings : TSettings;
        void AddCommand<TCommand>(string name) where TCommand : ICommand, ICommandLimiter<TSettings>;
    }
}