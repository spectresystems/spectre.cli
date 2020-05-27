using Spectre.Cli;

namespace Sample.Validation
{
    public sealed class ValidateProjectNameAttribute : ParameterValidationAttribute
    {
        public ValidateProjectNameAttribute() 
            : base(null)
        {
        }

        public override ValidationResult Validate(ICommandParameterInfo info, object value)
        {
            if (!(value is string project))
            {
                return ValidationResult.Error($"Package must be a string ({info?.PropertyName}).");
            }

            if (!project.EndsWith(".csproj"))
            {
                return ValidationResult.Error($"Provided project is not a csproj file ({info?.PropertyName}).");
            }

            return ValidationResult.Success();
        }
    }
}
