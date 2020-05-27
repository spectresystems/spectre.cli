namespace Spectre.Cli
{
    /// <summary>
    /// Represents a command parameter.
    /// </summary>
    public abstract class CommandParameterInfo
    {
        /// <summary>
        /// Gets the property name.
        /// </summary>
        /// <value>
        /// The property name.
        /// </value>
        public abstract string PropertyName { get; }

        /// <summary>
        /// Gets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public abstract string? Description { get; }
    }
}