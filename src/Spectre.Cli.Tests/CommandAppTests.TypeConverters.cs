using Shouldly;
using Spectre.Cli.Testing.Data.Commands;
using Spectre.Cli.Testing.Data.Settings;
using Spectre.Cli.Testing.Fakes;
using Xunit;

namespace Spectre.Cli.Tests
{
    public sealed partial class CommandAppTests
    {
        public sealed class TypeConverters
        {
            [Fact]
            public void Should_Bind_Using_Custom_Type_Converter_If_Specified()
            {
                // Given
                var resolver = new FakeTypeResolver();
                var settings = new CatSettings();
                resolver.Register(settings);

                var app = new CommandApp(new FakeTypeRegistrar(resolver));
                app.Configure(config =>
                {
                    config.AddCommand<CatCommand>("cat");
                });

                // When
                var result = app.Run(new[]
                {
                     "cat", "--name", "Tiger",
                     "--agility", "FOOBAR"
                });

                // Then
                result.ShouldBe(0);
                settings.Agility.ShouldBe(6);
            }
        }
    }
}
