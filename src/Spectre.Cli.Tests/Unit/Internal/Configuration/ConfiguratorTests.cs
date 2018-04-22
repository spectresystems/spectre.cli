using Shouldly;
using Spectre.Cli.Internal.Configuration;
using Spectre.Cli.Tests.Data;
using Spectre.Cli.Tests.Data.Settings;
using Xunit;

namespace Spectre.Cli.Tests.Unit.Internal.Configuration
{
    public sealed class ConfiguratorTests
    {
        [Fact]
        public void Should_Create_Configured_Commands_Correctly()
        {
            // Given
            var configurator = new Configurator(null);
            configurator.AddBranch<AnimalSettings>("animal", animal =>
            {
                animal.AddBranch<MammalSettings>("mammal", mammal =>
                {
                    mammal.AddCommand<DogCommand>("dog");
                    mammal.AddCommand<HorseCommand>("horse");
                });
            });

            // When
            var commands = configurator.Commands;

            // Then
            commands.Count.ShouldBe(1);
            commands[0].As(animal =>
            {
                animal.ShouldBeBranch<AnimalSettings>();
                animal.Children.Count.ShouldBe(1);

                animal.Children[0].As(mammal =>
                {
                    mammal.ShouldBeBranch<MammalSettings>();
                    mammal.Children.Count.ShouldBe(2);

                    mammal.Children[0].As(dog =>
                    {
                        dog.ShouldBeCommand<DogCommand, DogSettings>();
                        dog.Children.Count.ShouldBe(0);
                    });

                    mammal.Children[1].As(horse =>
                    {
                        horse.ShouldBeCommand<HorseCommand, MammalSettings>();
                        horse.Children.Count.ShouldBe(0);
                    });
                });
            });
        }
    }
}
