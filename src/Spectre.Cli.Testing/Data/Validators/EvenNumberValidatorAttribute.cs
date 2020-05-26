using System;

namespace Spectre.Cli.Testing.Data.Validators
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public sealed class EvenNumberValidatorAttribute : ParameterValidationAttribute
    {
        public EvenNumberValidatorAttribute(string errorMessage)
            : base(errorMessage)
        {
        }

        public override ValidationResult Validate(object value, string propertyName)
        {
            if (value is int integer)
            {
                if (integer % 2 == 0)
                {
                    return ValidationResult.Success();
                }

                return ValidationResult.Error($"Number is not even ({propertyName}).");
            }

            throw new InvalidOperationException($"Parameter is not a number ({propertyName}).");
        }
    }
}
