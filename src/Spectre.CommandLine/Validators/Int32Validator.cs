using System;

namespace Spectre.CommandLine.Validators
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public abstract class Int32Validator : ParameterValidationAttribute
    {
        protected Int32Validator(string message)
            : base(message)
        {
        }

        public sealed override ValidationResult Validate(object value)
        {
            if (value is int integer)
            {
                return Validate(integer);
            }
            throw new CommandAppException("Wrong type sent to validator.");
        }

        protected abstract ValidationResult Validate(int value);
    }
}
