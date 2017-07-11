namespace Spectre.CommandLine.Configuration
{
    internal abstract class CommandParameter
    {
        public ParameterInfo Info { get; }

        protected CommandParameter(ParameterInfo info)
        {
            Info = info;
        }

        public void Assign(object settings, object value)
        {
            Info.Property.SetValue(settings, value);
        }
    }
}