namespace Spectre.CommandLine
{
    public abstract class ProxyCommand : Command<NoSettings>
    {
        protected ProxyCommand(string name) 
            : base(name)
        {
        }

        public abstract override void Configure(ICommandRegistrar registrar);

        public sealed override int Run(NoSettings settings)
        {
            return 0;
        }
    }
}
