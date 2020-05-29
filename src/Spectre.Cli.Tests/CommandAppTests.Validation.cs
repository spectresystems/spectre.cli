using Shouldly;
using Spectre.Cli.Exceptions;
using Spectre.Cli.Testing;
using Spectre.Cli.Testing.Data.Commands;
using Spectre.Cli.Testing.Data.Settings;
using Spectre.Cli.Testing.Fakes;
using Xunit;

namespace Spectre.Cli.Tests
{
    public sealed partial class CommandAppTests
    {
        public sealed class Validation
        {
            [Fact]
            public void Should_Throw_If_Attribute_Validation_Fails()
            {
                // Given
                var app = new CommandApp(new FakeTypeRegistrar());
                app.Configure(config =>
                {
                    config.PropagateExceptions();
                    config.AddBranch<AnimalSettings>("animal", animal =>
                    {
                        animal.AddCommand<DogCommand>("dog");
                        animal.AddCommand<HorseCommand>("horse");
                    });
                });

                // When
                var result = Record.Exception(() => app.Run(new[] { "animal", "3", "dog", "7", "--name", "Rufus" }));

                // Then
                result.ShouldBeOfType<RuntimeException>().And(e =>
                {
                    e.Message.ShouldBe("Animals must have an even number of legs.");
                });
            }

            [Fact]
            public void Should_Throw_If_Settings_Validation_Fails()
            {
                // Given
                var app = new CommandApp(new FakeTypeRegistrar());
                app.Configure(config =>
                {
                    config.PropagateExceptions();
                    config.AddBranch<AnimalSettings>("animal", animal =>
                    {
                        animal.AddCommand<DogCommand>("dog");
                        animal.AddCommand<HorseCommand>("horse");
                    });
                });

                // When
                var result = Record.Exception(() => app.Run(new[] { "animal", "4", "dog", "7", "--name", "Tiger" }));

                // Then
                result.ShouldBeOfType<RuntimeException>().And(e =>
                {
                    e.Message.ShouldBe("Tiger is not a dog name!");
                });
            }

            [Fact]
            public void Should_Throw_If_Command_Validation_Fails()
            {
                // Given
                var app = new CommandApp(new FakeTypeRegistrar());
                app.Configure(config =>
                {
                    config.PropagateExceptions();
                    config.AddBranch<AnimalSettings>("animal", animal =>
                    {
                        animal.AddCommand<DogCommand>("dog");
                        animal.AddCommand<HorseCommand>("horse");
                    });
                });

                // When
                var result = Record.Exception(() => app.Run(new[] { "animal", "4", "dog", "101", "--name", "Rufus" }));

                // Then
                result.ShouldBeOfType<RuntimeException>().And(e =>
                {
                    e.Message.ShouldBe("Dog is too old...");
                });
            }
        }
    }
}
