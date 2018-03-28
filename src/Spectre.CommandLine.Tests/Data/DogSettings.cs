namespace Spectre.CommandLine.Tests.Data
{
    public sealed class DogSettings : MammalSettings, IValidate
    {
        [CommandArgument(0, "<AGE>")]
        public int Age { get; set; }

        [CommandOption("-g|--good-boy")]
        public bool GoodBoy { get; set; }

        public ValidationResult Validate()
        {
            if (Name == "Tiger")
            {
                return ValidationResult.Error("Tiger is not a dog name!");
            }
            return ValidationResult.Success;
        }
    }
}