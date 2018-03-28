using System;

namespace Spectre.CommandLine
{
    public sealed class ValidationResult
    {
        public bool Successful { get; }
        public string Message { get; }

        public static ValidationResult Success => new ValidationResult(true, null);

        private ValidationResult(bool successful, string message)
        {
            Successful = successful;
            Message = message;
        }

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