using System;

namespace Spectre.CommandLine
{
    public sealed class ValidationResult
    {
        public bool Successful { get; }
        public string Message { get; }

        private ValidationResult(bool successful, string message)
        {
            Successful = successful;
            Message = message;
        }

        public static ValidationResult Success()
        {
            return new ValidationResult(true, null);
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