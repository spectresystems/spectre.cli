using System;

namespace Spectre.Cli
{
    /// <summary>
    /// Implementation of a flag with an optional value.
    /// </summary>
    /// <typeparam name="T">The flag's element type.</typeparam>
    public sealed class FlagValue<T> : IFlagValue
    {
        /// <summary>
        /// Gets or sets a value indicating whether or not the flag was set or not.
        /// </summary>
        public bool IsSet { get; set; }

#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
        /// <summary>
        /// Gets or sets the flag's value.
        /// </summary>
        public T Value { get; set; }
#pragma warning restore CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.

        /// <inheritdoc/>
        Type IFlagValue.Type => typeof(T);

        /// <inheritdoc/>
        object? IFlagValue.Value
        {
            get => Value;
            set
            {
#pragma warning disable CS8601 // Possible null reference assignment.
                Value = (T)value;
#pragma warning restore CS8601 // Possible null reference assignment.
            }
        }
    }
}
