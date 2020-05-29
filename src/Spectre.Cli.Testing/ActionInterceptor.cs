using System;

namespace Spectre.Cli.Testing
{
    public sealed class ActionInterceptor : ICommandSettingsInterceptor
    {
        private readonly Action<CommandSettings> _action;

        public ActionInterceptor(Action<CommandSettings> action)
        {
            _action = action ?? throw new ArgumentNullException(nameof(action));
        }

        public void Intercept(CommandSettings settings)
        {
            _action(settings);
        }
    }
}
