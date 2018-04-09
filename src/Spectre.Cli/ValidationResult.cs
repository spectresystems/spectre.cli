using System;

namespace Spectre.Cli
{
    /// <summary>
    /// Represents a validation result.
    /// </summary>
    public sealed class ValidationResult
    {
        /// <summary>
        /// Gets a value indicating whether validation was successful.
        /// </summary>
        /// <value>
        ///   <c>true</c> if validation was successful; otherwise, <c>false</c>.
        /// </value>
        public bool Successful { get; }

        /// <summary>
        /// Gets the validation message.
        /// </summary>
        /// <value>The validation message.</value>
        public string Message { get; }

        private ValidationResult(bool successful, string message)
        {
            Successful = successful;
            Message = message;
        }

        /// <summary>
        /// Returns a successful validation result.
        /// </summary>
        /// <returns>A successful validation result</returns>
        public static ValidationResult Success()
        {
            return new ValidationResult(true, null);
        }

        /// <summary>
        /// Returns a validation error.
        /// </summary>
        /// <param name="message">The error message</param>
        /// <returns>A validation error.</returns>
        public static ValidationResult Error(string message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }
            return new ValidationResult(false, message);
        }
    }
}