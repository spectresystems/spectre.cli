namespace Spectre.CommandLine.Configuration
{
    internal abstract class CommandParameter
    {
        public ParameterInfo Parameter { get; }

        protected CommandParameter(ParameterInfo info)
        {
            Parameter = info;
        }

        public void Assign(object settings, object value)
        {
            Parameter.Property.SetValue(settings, value);
        }
    }
}