using System;
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
        public void Should_Create_Configured_Default_Command_If_Specified()
        {
            // Given, When
            var configurator = new Configurator(null);
            configurator.SetDefaultCommand<DogCommand>();

            // Then
            configurator.DefaultCommand.ShouldNotBeNull();
            configurator.DefaultCommand.As(command =>
            {
                command.Name.ShouldBe("__default_command");
                command.CommandType.ShouldBe<DogCommand>();
                command.SettingsType.ShouldBe<DogSettings>();
                command.Children.Count.ShouldBe(0);
                command.Description.ShouldBe(null);
            });
        }

        [Fact]
        public void Should_Create_Configured_Commands()
        {
            // Given, When
            var configurator = new Configurator(null);
            configurator.AddBranch<AnimalSettings>("animal", animal =>
            {
                animal.AddBranch<MammalSettings>("mammal", mammal =>
                {
                    mammal.AddCommand<DogCommand>("dog");
                    mammal.AddCommand<HorseCommand>("horse");
                });
            });

            // Then
            configurator.Commands.ShouldNotBeNull();
            configurator.Commands.Count.ShouldBe(1);
            configurator.Commands[0].As(animal =>
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
