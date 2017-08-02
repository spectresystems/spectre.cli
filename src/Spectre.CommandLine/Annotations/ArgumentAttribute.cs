using System;

namespace Spectre.CommandLine.Annotations
{
    /// <summary>
    /// Represents an argument.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class ArgumentAttribute : Attribute
    {
        public int Position { get; }
        public string ArgumentName { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ArgumentAttribute"/> class.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <param name="argumentName">The name of the argument.</param>
        public ArgumentAttribute(int position, string argumentName)
        {
            Position = position;
            ArgumentName = argumentName;
        }
    }
}
