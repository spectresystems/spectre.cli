using System;

namespace Spectre.Cli.Testing
{
    public sealed class ActionInterceptor : ICommandInterceptor
    {
        private readonly Action<CommandContext, CommandSettings> _action;

        public ActionInterceptor(Action<CommandContext, CommandSettings> action)
        {
            _action = action ?? throw new ArgumentNullException(nameof(action));
        }

        public void Intercept(CommandContext context, CommandSettings settings)
        {
            _action(context, settings);
        }
    }
}
