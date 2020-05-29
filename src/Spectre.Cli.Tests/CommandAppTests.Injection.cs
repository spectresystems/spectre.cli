using Shouldly;
using Spectre.Cli.Testing;
using Spectre.Cli.Testing.Data.Commands;
using Spectre.Cli.Testing.Fakes;
using Xunit;

namespace Spectre.Cli.Tests
{
    public sealed partial class CommandAppTests
    {
        public sealed class Injection
        {
            public sealed class FakeDependency
            {
            }

            public sealed class Settings : CommandSettings
            {
                public FakeDependency Fake { get; set; }

                [CommandOption("--name <NAME>")]
                public string Name { get; }

                [CommandOption("--age <AGE>")]
                public int Age { get; set; }

                public Settings(FakeDependency fake, string name)
                {
                    Fake = fake;
                    Name = "Hello " + name;
                }
            }

            [Fact]
            public void Should_Inject_Parameters()
            {
                // Given
                var resolver = new FakeTypeResolver();
                var dependency = new FakeDependency();
                resolver.Register(dependency);

                var app = new CommandApp<GenericCommand<Settings>>(new FakeTypeRegistrar(resolver));
                var settings = default(Settings);
                app.Configure(config =>
                {
                    config.SetInterceptor(new ActionInterceptor(intercepted => settings = (Settings)intercepted));
                    config.PropagateExceptions();
                });

                // When
                var result = app.Run(new[]
                {
                    "--name", "foo",
                    "--age", "35",
                });

                // Then
                result.ShouldBe(0);
                settings.ShouldNotBeNull();
                settings.Fake.ShouldBe(dependency);
                settings.Name.ShouldBe("Hello foo");
                settings.Age.ShouldBe(35);
            }
        }
    }
}
