# Spectre.CommandLine

An extremly opinionated command line parser.

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
        using (var app = new CommandApp())
        {
            app.Configure(config =>
            {
                config.AddProxy<EFSettings>("ef", ef => 
                {
                    ef.AddProxy<EFDatabaseSettings>("database", database =>
                    {
                        database.AddCommand<EFUpdateCommand>("update");
                        database.AddCommand<EFDropCommand>("drop");
                    }
                });
            });

            return app.Run(args);
        }
    }
}
```

You can now execute the `drop` command like this:

```
./fakedotnet.exe ef --verbose database drop --startup-project "./../Foo/Foo.csproj"
```

See the `samples` directory for more information.