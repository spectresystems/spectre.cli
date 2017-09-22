// Licensed to Spectre Systems AB under one or more agreements.
// Spectre Systems AB licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace Spectre.CommandLine
{
    /// <summary>
    /// Represents errors that occur in the command application.
    /// </summary>
    /// <seealso cref="System.Exception" />
    public sealed class CommandAppException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommandAppException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public CommandAppException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandAppException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public CommandAppException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
