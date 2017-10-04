using Sample.EF;
using Sample.EF.Commands;
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
                config.AddProxy<EfSettings>("ef", ef =>
                {
                    ef.SetDescription("Fake EF Core .NET Command Line Tools");
                    ef.AddProxy<EfDatabaseSettings>("database", database =>
                    {
                        database.AddCommand<EfUpdateCommand>("update");
                        database.AddCommand<EfDropCommand>("drop");
                    });
                });
            });

            return app.Run(args);
        }
    }
}


