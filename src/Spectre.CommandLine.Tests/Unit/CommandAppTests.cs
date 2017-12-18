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
            var resolver = new FakeTypeResolver();
            var settings = new DogSettings();
            resolver.Register(settings);

            var app = new CommandApp();
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
            var result = app.Run(
                new[] { "animal", "--alive", "mammal", "--name", "Rufus", "dog", "12", "--good-boy" },
                resolver);

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
            var resolver = new FakeTypeResolver();
            var settings = new DogSettings();
            resolver.Register(settings);

            var app = new CommandApp();
            app.Configure(config =>
            {
                config.AddCommand<DogCommand>("dog");
            });

            // When
            var result = app.Run(
                new[] { "dog", "12", "--good-boy", "--name", "Rufus", "--alive" },
                resolver);

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
            var resolver = new FakeTypeResolver();
            var settings = new DogSettings();
            resolver.Register(settings);

            var app = new CommandApp();
            app.Configure(config =>
            {
                config.AddCommand<AnimalSettings>("animal", animal =>
                {
                    animal.AddCommand<DogCommand>("dog");
                    animal.AddCommand<HorseCommand>("horse");
                });
            });

            // When
            var result = app.Run(
                new[] { "animal", "dog", "12", "--good-boy", "--name", "Rufus" },
                resolver);

            // Then
            result.ShouldBe(0);
            settings.Age.ShouldBe(12);
            settings.GoodBoy.ShouldBe(true);
            settings.IsAlive.ShouldBe(false);
            settings.Name.ShouldBe("Rufus");
        }

        [Fact]
        public void Should_Register_Commands_When_Configuring_Application()
        {
            // Given
            var registrar = new FakeTypeRegistrar();
            var app = new CommandApp(registrar);

            // When
            app.Configure(config =>
            {
                config.AddCommand<AnimalSettings>("animal", animal =>
                {
                    animal.AddCommand<DogCommand>("dog");
                    animal.AddCommand<HorseCommand>("horse");
                });
            });

            // Then
            registrar.Registrations.ContainsKey(typeof(ICommand)).ShouldBeTrue();
            registrar.Registrations[typeof(ICommand)].Count.ShouldBe(2);
            registrar.Registrations[typeof(ICommand)].ShouldContain(typeof(DogCommand));
            registrar.Registrations[typeof(ICommand)].ShouldContain(typeof(HorseCommand));
        }

        [Fact]
        public void Should_Register_Command_Settings_When_Configuring_Application()
        {
            // Given
            var registrar = new FakeTypeRegistrar();
            var app = new CommandApp(registrar);

            // When
            app.Configure(config =>
            {
                config.AddCommand<AnimalSettings>("animal", animal =>
                {
                    animal.AddCommand<DogCommand>("dog");
                    animal.AddCommand<HorseCommand>("horse");
                });
            });

            // Then
            registrar.Registrations.ContainsKey(typeof(DogSettings)).ShouldBeTrue();
            registrar.Registrations[typeof(DogSettings)].Count.ShouldBe(1);
            registrar.Registrations[typeof(DogSettings)].ShouldContain(typeof(DogSettings));
            registrar.Registrations.ContainsKey(typeof(MammalSettings)).ShouldBeTrue();
            registrar.Registrations[typeof(MammalSettings)].Count.ShouldBe(1);
            registrar.Registrations[typeof(MammalSettings)].ShouldContain(typeof(MammalSettings));
        }
    }
}
