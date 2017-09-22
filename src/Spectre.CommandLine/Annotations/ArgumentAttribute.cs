// Licensed to Spectre Systems AB under one or more agreements.
// Spectre Systems AB licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace Spectre.CommandLine.Annotations
{
    /// <summary>
    /// Represents an argument.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class ArgumentAttribute : Attribute
    {
        /// <summary>
        /// Gets the argument position.
        /// </summary>
        /// <value>The argument position.</value>
        public int Position { get; }

        /// <summary>
        /// Gets the name of the argument.
        /// </summary>
        /// <value>The name of the argument.</value>
        public string ArgumentName { get; }

        /// <summary>
        /// Gets a value indicating whether this argument is required.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this argument is required; otherwise, <c>false</c>.
        /// </value>
        public bool IsRequired { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ArgumentAttribute"/> class.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <param name="argumentName">The name of the argument.</param>
        public ArgumentAttribute(int position, string argumentName)
        {
            Position = position;
            IsRequired = argumentName.StartsWith("[") && argumentName.EndsWith("]");
            ArgumentName = argumentName.Trim('[', ']', '<', '>');
        }
    }
}
