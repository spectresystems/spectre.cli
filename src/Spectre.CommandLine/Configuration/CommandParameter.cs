namespace Spectre.CommandLine.Configuration
{
    internal abstract class CommandParameter
    {
        public ParameterInfo Parameter { get; }
        public bool IsRequired { get; set; }

        protected CommandParameter(ParameterInfo info, bool required)
        {
            Parameter = info;
            IsRequired = required;
        }

        public void Assign(object settings, object value)
        {
            Parameter.Property.SetValue(settings, value);
        }
    }
}