using FakeDotNet.EF;
using FakeDotNet.EF.Database;
using FakeDotNet.EF.DbContext;
using Spectre.Cli;

namespace FakeDotNet
{
    public static class Program
    {
        public static int Main(string[] args)
        {
            var app = new CommandApp();
            app.Configure(config =>
            {
                config.SetApplicationName("fakedotnet");

                // Root command
                config.AddCommand<EfSettings>("ef", ef =>
                {
                    ef.SetDescription("Fake EF Core .NET Command Line Tools");

                    // Database
                    ef.AddCommand<EfCommandSettings>("database", database =>
                    {
                        database.AddCommand<EfUpdateCommand>("update");
                        database.AddCommand<EfDropCommand>("drop");
                    });

                    // DbContext
                    ef.AddCommand<EfCommandSettings>("dbcontext", dbcontext =>
                    {
                        dbcontext.AddCommand<EfScaffoldCommand>("scaffold");
                    });
                });
            });

            return app.Run(args);
        }
    }
}
