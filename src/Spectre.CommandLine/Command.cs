namespace Spectre.CommandLine
{
    public abstract class Command : Command<NoSettings>
    {
        protected Command(string name) 
            : base(name)
        {
        }

        public abstract int Run();

        public sealed override int Run(NoSettings settings)
        {
            return Run();
        }
    }
}