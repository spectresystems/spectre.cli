using System;
using Spectre.CommandLine.Validators;

namespace Spectre.CommandLine.Tests.Data.Validators
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public sealed class EvenNumberValidatorAttribute : Int32Validator
    {
        public EvenNumberValidatorAttribute(string message)
            : base(message)
        {
        }

        protected override ValidationResult Validate(int value)
        {
            if (value % 2 == 0)
            {
                return ValidationResult.Success;
            }
            return ValidationResult.Error("Number is not even.");
        }
    }
}
