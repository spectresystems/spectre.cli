using Spectre.Cli;

namespace Sample.Validation
{
    public sealed class ValidateProjectNameAttribute : ParameterValidationAttribute
    {
        public ValidateProjectNameAttribute() 
            : base(null)
        {
        }

        public override ValidationResult Validate(object value, CommandParameterInfo parameterInfo)
        {
            if (!(value is string project))
            {
                return ValidationResult.Error($"Package must be a string ({parameterInfo?.PropertyName}).");
            }

            if (!project.EndsWith(".csproj"))
            {
                return ValidationResult.Error($"Provided project is not a csproj file ({parameterInfo?.PropertyName}).");
            }

            return ValidationResult.Success();
        }
    }
}
