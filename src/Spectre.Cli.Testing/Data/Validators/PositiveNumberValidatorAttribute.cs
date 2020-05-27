using System;

namespace Spectre.Cli.Testing.Data.Validators
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public sealed class PositiveNumberValidatorAttribute : ParameterValidationAttribute
    {
        public PositiveNumberValidatorAttribute(string errorMessage)
            : base(errorMessage)
        {
        }

        public override ValidationResult Validate(object value, CommandParameterInfo parameterInfo)
        {
            if (value is int integer)
            {
                if (integer > 0)
                {
                    return ValidationResult.Success();
                }

                return ValidationResult.Error($"Number is not greater than 0 ({parameterInfo?.PropertyName}).");
            }

            throw new InvalidOperationException($"Parameter is not a number ({parameterInfo?.PropertyName}).");
        }
    }
}