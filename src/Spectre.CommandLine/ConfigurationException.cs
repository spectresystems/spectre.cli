using System;
using System.Runtime.Serialization;

namespace Spectre.CommandLine
{
    /// <summary>
    /// Represents errors that occur during configuration of the application.
    /// </summary>
    /// <seealso cref="Exception" />
    [Serializable]
    public sealed class ConfigurationException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationException"/> class.
        /// </summary>
        public ConfigurationException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public ConfigurationException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationException"/> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
        public ConfigurationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        private ConfigurationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}