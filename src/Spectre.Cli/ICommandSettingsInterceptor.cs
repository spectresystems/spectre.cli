namespace Spectre.Cli
{
    /// <summary>
    /// Represents a command settings interceptor that
    /// will intercept command settings before it's
    /// passed to a command.
    /// </summary>
    public interface ICommandSettingsInterceptor
    {
        /// <summary>
        /// Intercepts command settings before it's passed to a command.
        /// </summary>
        /// <param name="settings">The intercepted <see cref="CommandSettings"/>.</param>
        void Intercept(CommandSettings settings);
    }
}
