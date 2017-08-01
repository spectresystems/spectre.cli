# Spectre.CommandLine

An extremly opinionated command line parser.

## Usage

Here is a short example of how to configure the command chain.
By using some clever generic constraints, the framework guarantees that all settings associated 
with a command, is inherited from the parent commands' settings.

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
                config.AddProxy<FooSettings>("foo", foo =>
                {
                    foo.AddCommand<BarCommand>("bar");                    
                    foo.AddProxy<BazSettings>("baz", baz =>
                    {
                        baz.AddCommand<QuxCommand>("qux");
                    });
                });
            });

            return app.Run(args);
        }
    }
}
```

You can now execute the `Baz` command like this:

```
myapp foo --foo Hello baz --baz 101 --qux "2017-07-06 15:21"
```

Or like this:

```
myapp foo --foo Hello bar --bar 101
```

Since everything is strongly typed, and the generic parameter type 
for `AddProxy`/`AddCommand` is enforced by the compiler, everything 
in `FooSettings` will be available in the `BarCommand`.

See the `samples` directory for more information.