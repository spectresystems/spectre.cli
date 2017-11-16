using Sample.EF;
using Sample.EF.Database;
using Sample.EF.DbContext;
using Spectre.CommandLine;

namespace Sample
{
    public class Program
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
