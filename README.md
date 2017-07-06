# Spectre.CommandLine

A wrapper library for Microsoft.Extensions.CommandLineUtils.

## Usage

Here is a short example of how to configure the command chain.
By using some clever generic constraints, the framework guarantees that all settings associated 
with a command, is inherited from the parent commands' settings.

```csharp
using Example.Commands;
using Spectre.CommandLine;

namespace Example
{
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
                        foo.AddCommand<BazCommand>("baz");
                    });
                });

                return app.Run(args);
            }
        }
    }
}
```

This is implemented as following:

```csharp
public abstract class FooSettings
{
    [Option("-f|--foo")]
    [Description("Essential to enable fooing of the bar or baz.")]
    public string Foo { get; set; }
}

public sealed class BarCommand : Command<BarCommand.Settings>
{
    public sealed class Settings : FooSettings
    {
        [Option("-b|--bar")]
        [Description("Triggers a bar.")]
        [DefaultValue(99)]
        public int Bar { get; set; }
    }

    public override int Run(Settings settings)
    {
        Console.WriteLine($"Foo={settings.Foo} Bar={settings.Bar}");
        return 0;
    }
}

public sealed class BazCommand : Command<BazCommand.Settings>
{
    public sealed class Settings : FooSettings
    {
        [Option("-b|--baz")]
        [Description("Re-enables the baz in all sub systems.")]
        public int Baz { get; set; }
    }

    public override int Run(Settings settings)
    {
        Console.WriteLine($"Foo={settings.Foo} Baz={settings.Baz}");
        return 0;
    }
}

public abstract class BazSettings : FooSettings
{
    [Option("-b|--baz")]
    [Description("Re-enables the baz in all sub systems.")]
    public string Baz { get; set; }
}

public sealed class QuxCommand : Command<QuxCommand.Settings>
{
    public sealed class Settings : BazSettings
    {
        [Option("-q|--qux")]
        [Description("Sets the qux timestamp for the current baz.")]
        public DateTime Qux { get; set; }
    }

    public override int Run(Settings settings)
    {
        Console.WriteLine($"Foo={settings.Foo} Baz={settings.Baz} Qux={settings.Qux}");
        return 0;
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