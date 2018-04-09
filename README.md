# Spectre.CommandLine

An extremly opinionated command line parser.

[![NuGet](https://img.shields.io/nuget/v/Spectre.CommandLine.svg)](https://www.nuget.org/packages/Spectre.CommandLine)

[![Build status](https://ci.appveyor.com/api/projects/status/1johjx7tjvux4qb4?svg=true)](https://ci.appveyor.com/project/patriksvensson/spectre-commandline)

## Usage

Below is a short example of how to configure the command chain.
By using some clever generic constraints, the framework guarantees that all settings associated 
with a command is inherited from the parent commands' settings.

```csharp
using Spectre.CommandLine;

public class Program
{
    public static int Main(string[] args)
    {
        var app = new CommandApp();

        app.Configure(config =>
        {
            config.AddCommand<EfSettings>("ef", ef =>
            {
                ef.SetDescription("Fake EF Core .NET Command Line Tools");
                ef.AddCommand<EfDatabaseSettings>("database", database =>
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