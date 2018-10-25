namespace Spectre.Cli
{
    /// <summary>
    /// Represents a command configurator.
    /// </summary>
    public interface ICommandConfigurator
    {
        /// <summary>
        /// Adds an example of how to use the command.
        /// </summary>
        /// <param name="args">The example arguments.</param>
        /// <returns>The same <see cref="ICommandConfigurator"/> instance so that multiple calls can be chained.</returns>
        ICommandConfigurator WithExample(params string[] args);
    }
}
