using Shouldly;
using Spectre.CommandLine.Tests.Data;
using Spectre.CommandLine.Tests.Fakes;
using Xunit;

namespace Spectre.CommandLine.Tests.Unit
{
    public sealed class CommandAppTests
    {
        [Fact]
        public void Should_Pass_Case_1()
        {
            // Given
            var activator = new FakeTypeResolver();
            var settings = new DogSettings();
            activator.Register(settings);

            var app = new CommandApp(activator);
            app.Configure(config =>
            {
                config.AddCommand<AnimalSettings>("animal", animal =>
                {
                    animal.AddCommand<MammalSettings>("mammal", mammal =>
                    {
                        mammal.AddCommand<DogCommand>("dog");
                        mammal.AddCommand<HorseCommand>("horse");
                    });
                });
            });

            // When
            var result = app.Run(new[] { "animal", "--alive", "mammal", "--name", "Rufus", "dog", "12", "--good-boy" });

            // Then
            result.ShouldBe(0);
            settings.Age.ShouldBe(12);
            settings.GoodBoy.ShouldBe(true);
            settings.IsAlive.ShouldBe(true);
            settings.Name.ShouldBe("Rufus");
        }

        [Fact]
        public void Should_Pass_Case_2()
        {
            // Given
            var activator = new FakeTypeResolver();
            var settings = new DogSettings();
            activator.Register(settings);

            var app = new CommandApp(activator);
            app.Configure(config =>
            {
                config.AddCommand<DogCommand>("dog");
            });

            // When
            var result = app.Run(new[] { "dog", "12", "--good-boy", "--name", "Rufus", "--alive" });

            // Then
            result.ShouldBe(0);
            settings.Age.ShouldBe(12);
            settings.GoodBoy.ShouldBe(true);
            settings.IsAlive.ShouldBe(true);
            settings.Name.ShouldBe("Rufus");
        }

        [Fact]
        public void Should_Pass_Case_3()
        {
            // Given
            var activator = new FakeTypeResolver();
            var settings = new DogSettings();
            activator.Register(settings);

            var app = new CommandApp(activator);
            app.Configure(config =>
            {
                config.AddCommand<AnimalSettings>("animal", animal =>
                {
                    animal.AddCommand<DogCommand>("dog");
                    animal.AddCommand<HorseCommand>("horse");
                });
            });

            // When
            var result = app.Run(new[] { "animal", "dog", "12", "--good-boy", "--name", "Rufus" });

            // Then
            result.ShouldBe(0);
            settings.Age.ShouldBe(12);
            settings.GoodBoy.ShouldBe(true);
            settings.IsAlive.ShouldBe(false);
            settings.Name.ShouldBe("Rufus");
        }
    }
}
