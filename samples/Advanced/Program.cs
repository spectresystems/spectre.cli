using System.Threading.Tasks;
using Autofac;
using Sample.Autofac;
using Sample.Commands;
using Spectre.Cli;

namespace Sample
{
    public static class Program
    {
        public static async Task<int> Main(string[] args)
        {
            var registrar = new AutofacTypeRegistrar(BuildContainer());
            var app = new CommandApp<BuildCommand>(registrar);

            app.Configure(config =>
            {
                config.SetApplicationName("advanced");
                config.AddCommand<BuildCommand>("build");
            });

            return await app.RunAsync(args);
        }

        private static ContainerBuilder BuildContainer()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<FileSystem>().As<IFileSystem>().SingleInstance();
            return builder;
        }
    }
}
