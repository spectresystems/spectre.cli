using System;
using Spectre.CommandLine.Validators;

namespace Spectre.CommandLine.Tests.Data.Validators
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public sealed class PositiveNumberValidatorAttribute : Int32Validator
    {
        public PositiveNumberValidatorAttribute(string message)
            : base(message)
        {
        }

        protected override ValidationResult Validate(int value)
        {
            if (value > 0)
            {
                return ValidationResult.Success();
            }
            return ValidationResult.Error("Number is not greater than 0.");
        }
    }
}