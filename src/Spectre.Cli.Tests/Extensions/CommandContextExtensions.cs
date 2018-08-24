using System.Linq;
using Shouldly;

namespace Spectre.Cli.Tests
{
    public static class CommandContextExtensions
    {
        public static void ShouldHaveRemainingArgument(this CommandContext context, string name, string[] values)
        {
            context.Remaining.Parsed.Contains(name).ShouldBeTrue();
            context.Remaining.Parsed[name].Count().ShouldBe(values.Length);

            foreach (var value in values)
            {
                context.Remaining.Parsed[name].ShouldContain(value);
            }
        }
    }
}
