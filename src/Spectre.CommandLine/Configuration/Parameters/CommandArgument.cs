namespace Spectre.CommandLine.Configuration.Parameters
{
    internal sealed class CommandArgument : CommandParameter
    {
        public int Position { get; set; }
        public string Name { get; }

        public CommandArgument(ParameterInfo info, int position, string name) 
            : base(info)
        {
            Position = position;
            Name = name;
        }
    }
}
