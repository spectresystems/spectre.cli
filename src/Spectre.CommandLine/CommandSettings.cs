namespace Spectre.CommandLine
{
    public abstract class CommandSettings
    {
        public virtual ValidationResult Validate()
        {
            return ValidationResult.Success();
        }
    }
}
