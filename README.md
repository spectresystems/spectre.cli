# Spectre.Cli

An extremly opinionated command line parser targeting [.NET Standard 2.0](https://docs.microsoft.com/en-us/dotnet/standard/net-standard#net-implementation-support).

[![NuGet](https://img.shields.io/nuget/v/Spectre.Cli.svg)](https://www.nuget.org/packages/Spectre.Cli) ![Build Status](https://ci.appveyor.com/api/projects/status/1johjx7tjvux4qb4?svg=true)

## Usage

Below is a short example of how to configure the command chain.
By using some clever generic constraints, the framework guarantees that all settings associated 
with a command is inherited from the parent commands' settings.

```csharp
using Spectre.Cli;

public class Program
{
    public static int Main(string[] args)
    {
        var app = new CommandApp();

        app.Configure(config =>
        {
            config.AddBranch<EfSettings>("ef", ef =>
            {
                ef.SetDescription("Fake EF Core .NET Command Line Tools");
                ef.AddBranch<EfDatabaseSettings>("database", database =>
                {
                    database.AddCommand<EfUpdateCommand>("update");
                    database.AddCommand<EfDropCommand>("drop");
                });
            });
        });

        return app.Run(args);
    }
}
```

You can now execute the `drop` command like this:

```
./fakedotnet.exe ef --verbose database --no-color drop --startup-project "./../Foo/Foo.csproj" --dry-run
```

See the `samples` directory for more information.