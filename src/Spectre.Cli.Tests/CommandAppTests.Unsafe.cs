using Shouldly;
using Spectre.Cli.Exceptions;
using Spectre.Cli.Testing;
using Spectre.Cli.Testing.Data.Commands;
using Spectre.Cli.Testing.Data.Settings;
using Spectre.Cli.Testing.Fakes;
using Spectre.Cli.Unsafe;
using Xunit;

namespace Spectre.Cli.Tests
{
    public sealed partial class CommandAppTests
    {
        public sealed class SafetyOff
        {
            [Fact]
            public void Can_Mix_Safe_And_Unsafe_Configurators()
            {
                // Given
                var resolver = new FakeTypeResolver();
                var settings = new DogSettings();
                resolver.Register(settings);

                var app = new CommandApp(new FakeTypeRegistrar(resolver));
                app.Configure(config =>
                {
                    config.PropagateExceptions();

                    config.AddBranch<AnimalSettings>("animal", animal =>
                    {
                        animal.SafetyOff().AddBranch("mammal", typeof(MammalSettings), mammal =>
                        {
                            mammal.AddCommand("dog", typeof(DogCommand));
                            mammal.AddCommand("horse", typeof(HorseCommand));
                        });
                    });
                });

                // When
                var result = app.Run(new[]
                {
                    "animal", "--alive", "mammal", "--name",
                    "Rufus", "dog", "12", "--good-boy",
                });

                // Then
                result.ShouldBe(0);
                settings.Age.ShouldBe(12);
                settings.GoodBoy.ShouldBe(true);
                settings.Name.ShouldBe("Rufus");
                settings.IsAlive.ShouldBe(true);
            }

            [Fact]
            public void Can_Turn_Safety_On_After_Turning_It_Off_For_Branch()
            {
                // Given
                var resolver = new FakeTypeResolver();
                var settings = new DogSettings();
                resolver.Register(settings);

                var app = new CommandApp(new FakeTypeRegistrar(resolver));
                app.Configure(config =>
                {
                    config.PropagateExceptions();

                    config.SafetyOff().AddBranch("animal", typeof(AnimalSettings), animal =>
                    {
                        animal.SafetyOn<AnimalSettings>()
                            .AddBranch<MammalSettings>("mammal", mammal =>
                        {
                            mammal.SafetyOff().AddCommand("dog", typeof(DogCommand));
                            mammal.AddCommand<HorseCommand>("horse");
                        });
                    });
                });

                // When
                var result = app.Run(new[]
                {
                    "animal", "--alive", "mammal", "--name",
                    "Rufus", "dog", "12", "--good-boy",
                });

                // Then
                result.ShouldBe(0);
                settings.Age.ShouldBe(12);
                settings.GoodBoy.ShouldBe(true);
                settings.Name.ShouldBe("Rufus");
                settings.IsAlive.ShouldBe(true);
            }

            [Fact]
            public void Should_Throw_If_Trying_To_Convert_Unsafe_Branch_Configurator_To_Safe_Version_With_Wrong_Type()
            {
                // Given
                var app = new CommandApp();

                // When
                var result = Record.Exception(() => app.Configure(config =>
                {
                    config.PropagateExceptions();

                    config.SafetyOff().AddBranch("animal", typeof(AnimalSettings), animal =>
                    {
                        animal.SafetyOn<MammalSettings>().AddCommand<DogCommand>("dog");
                    });
                }));

                // Then
                result.ShouldBeOfType<ConfigurationException>();
                result.Message.ShouldBe("Configurator cannot be converted to a safe configurator of type 'MammalSettings'.");
            }

            [Fact]
            public void Should_Pass_Case_1()
            {
                // Given
                var resolver = new FakeTypeResolver();
                var settings = new DogSettings();
                resolver.Register(settings);

                var app = new CommandApp(new FakeTypeRegistrar(resolver));
                app.Configure(config =>
                {
                    config.PropagateExceptions();

                    config.SafetyOff().AddBranch("animal", typeof(AnimalSettings), animal =>
                    {
                        animal.AddBranch("mammal", typeof(MammalSettings), mammal =>
                        {
                            mammal.AddCommand("dog", typeof(DogCommand));
                            mammal.AddCommand("horse", typeof(HorseCommand));
                        });
                    });
                });

                // When
                var result = app.Run(new[]
                {
                    "animal", "--alive", "mammal", "--name",
                    "Rufus", "dog", "12", "--good-boy",
                });

                // Then
                result.ShouldBe(0);
                settings.Age.ShouldBe(12);
                settings.GoodBoy.ShouldBe(true);
                settings.Name.ShouldBe("Rufus");
                settings.IsAlive.ShouldBe(true);
            }

            [Fact]
            public void Should_Pass_Case_2()
            {
                // Given
                var resolver = new FakeTypeResolver();
                var settings = new DogSettings();
                resolver.Register(settings);

                var app = new CommandApp(new FakeTypeRegistrar(resolver));
                app.Configure(config =>
                {
                    config.PropagateExceptions();
                    config.SafetyOff().AddCommand("dog", typeof(DogCommand));
                });

                // When
                var result = app.Run(new[] { "dog", "12", "4", "--good-boy", "--name", "Rufus", "--alive" });

                // Then
                result.ShouldBe(0);
                settings.Legs.ShouldBe(12);
                settings.Age.ShouldBe(4);
                settings.GoodBoy.ShouldBe(true);
                settings.Name.ShouldBe("Rufus");
                settings.IsAlive.ShouldBe(true);
            }

            [Fact]
            public void Should_Pass_Case_3()
            {
                // Given
                var resolver = new FakeTypeResolver();
                var settings = new DogSettings();
                resolver.Register(settings);

                var app = new CommandApp(new FakeTypeRegistrar(resolver));
                app.Configure(config =>
                {
                    config.PropagateExceptions();
                    config.SafetyOff().AddBranch("animal", typeof(AnimalSettings), animal =>
                    {
                        animal.AddCommand("dog", typeof(DogCommand));
                        animal.AddCommand("horse", typeof(HorseCommand));
                    });
                });

                // When
                var result = app.Run(new[] { "animal", "dog", "12", "--good-boy", "--name", "Rufus" });

                // Then
                result.ShouldBe(0);
                settings.Age.ShouldBe(12);
                settings.GoodBoy.ShouldBe(true);
                settings.Name.ShouldBe("Rufus");
                settings.IsAlive.ShouldBe(false);
            }

            [Fact]
            public void Should_Pass_Case_4()
            {
                // Given
                var resolver = new FakeTypeResolver();
                var settings = new DogSettings();
                resolver.Register(settings);

                var app = new CommandApp(new FakeTypeRegistrar(resolver));
                app.Configure(config =>
                {
                    config.PropagateExceptions();
                    config.SafetyOff().AddBranch("animal", typeof(AnimalSettings), animal =>
                    {
                        animal.AddCommand("dog", typeof(DogCommand));
                    });
                });

                // When
                var result = app.Run(new[] { "animal", "4", "dog", "12", "--good-boy", "--name", "Rufus" });

                // Then
                result.ShouldBe(0);
                settings.Legs.ShouldBe(4);
                settings.Age.ShouldBe(12);
                settings.GoodBoy.ShouldBe(true);
                settings.IsAlive.ShouldBe(false);
                settings.Name.ShouldBe("Rufus");
            }

            [Fact]
            public void Should_Pass_Case_5()
            {
                // Given
                var resolver = new FakeTypeResolver();
                var settings = new OptionVectorSettings();
                resolver.Register(settings);

                var app = new CommandApp(new FakeTypeRegistrar(resolver));
                app.Configure(config =>
                {
                    config.PropagateExceptions();
                    config.SafetyOff().AddCommand("multi", typeof(OptionVectorCommand));
                });

                // When
                var result = app.Run(new[] { "multi", "--foo", "a", "--foo", "b", "--bar", "1", "--foo", "c", "--bar", "2" });

                // Then
                result.ShouldBe(0);
                settings.Foo.Length.ShouldBe(3);
                settings.Foo.ShouldBe(new[] { "a", "b", "c" });
                settings.Bar.Length.ShouldBe(2);
                settings.Bar.ShouldBe(new[] { 1, 2 });
            }

            [Fact]
            public void Should_Pass_Case_6()
            {
                // Given
                var resolver = new FakeTypeResolver();
                var settings = new ArgumentVectorSettings();
                resolver.Register(settings);

                var app = new CommandApp(new FakeTypeRegistrar(resolver));
                app.Configure(config =>
                {
                    config.PropagateExceptions();
                    config.AddCommand<GenericCommand<ArgumentVectorSettings>>("multi");
                });

                // When
                var result = app.Run(new[] { "multi", "a", "b", "c" });

                // Then
                result.ShouldBe(0);
                settings.Foo.Length.ShouldBe(3);
                settings.Foo.ShouldBe(new[] { "a", "b", "c" });
            }
        }
    }
}
