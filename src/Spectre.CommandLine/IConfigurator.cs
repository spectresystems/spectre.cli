using System;

namespace Spectre.CommandLine
{
    public interface IConfigurator
    {
        void SetApplicationName(string name);
        void PropagateExceptions();

        void AddCommand<TCommand>(string name) where TCommand : class, ICommand;
        void AddCommand<TSettings>(string name, Action<IConfigurator<TSettings>> action) where TSettings : CommandSettings;
    }

    public interface IConfigurator<in TSettings>
        where TSettings : CommandSettings
    {
        void SetDescription(string description);

        void AddCommand<TCommand>(string name) where TCommand : class, ICommandLimiter<TSettings>;
        void AddCommand<TDerivedSettings>(string name, Action<IConfigurator<TDerivedSettings>> action) where TDerivedSettings : TSettings;
    }
}
