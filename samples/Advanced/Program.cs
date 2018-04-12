using System.Threading.Tasks;
using Advanced.Cli.Autofac;
using Advanced.Cli.Commands;
using Advanced.Utilities;
using Autofac;
using Spectre.Cli;

namespace Advanced
{
    public static class Program
    {
        public static async Task<int> Main(string[] args)
        {
            var registrar = new AutofacTypeRegistrar(BuildContainer());
            var app = new CommandApp(registrar);

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
            builder.RegisterType<Clock>().As<IClock>().SingleInstance();
            builder.RegisterType<FileSystem>().As<IFileSystem>().SingleInstance();
            return builder;
        }
    }
}
