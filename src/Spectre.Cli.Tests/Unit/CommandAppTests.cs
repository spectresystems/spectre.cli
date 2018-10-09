using System.Threading.Tasks;
using Shouldly;
using Spectre.Cli.Internal.Exceptions;
using Spectre.Cli.Tests.Data;
using Spectre.Cli.Tests.Data.Settings;
using Spectre.Cli.Tests.Fakes;
using Xunit;

namespace Spectre.Cli.Tests.Unit
{
    public sealed class CommandAppTests
    {
        [Fact]
        public async Task Should_Pass_Case_1()
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
                    animal.AddBranch<MammalSettings>("mammal", mammal =>
                    {
                        mammal.AddCommand<DogCommand>("dog");
                        mammal.AddCommand<HorseCommand>("horse");
                    });
                });
            });

            // When
            var result = await app.RunAsync(
                new[] { "animal", "--alive", "mammal", "--name", "Rufus", "dog", "12", "--good-boy" });

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

            var app = new CommandApp(new FakeTypeRegistrar(resolver));
            app.Configure(config =>
            {
                config.PropagateExceptions();
                config.AddCommand<DogCommand>("dog");
            });

            // When
            var result = app.Run(new[] { "dog", "12", "4", "--good-boy", "--name", "Rufus", "--alive" });

            // Then
            result.ShouldBe(0);
            settings.Legs.ShouldBe(12);
            settings.Age.ShouldBe(4);
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

            var app = new CommandApp(new FakeTypeRegistrar(resolver));
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
            var result = app.Run(new[] { "animal", "dog", "12", "--good-boy", "--name", "Rufus" });

            // Then
            result.ShouldBe(0);
            settings.Age.ShouldBe(12);
            settings.GoodBoy.ShouldBe(true);
            settings.IsAlive.ShouldBe(false);
            settings.Name.ShouldBe("Rufus");
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
                config.AddBranch<AnimalSettings>("animal", animal =>
                {
                    animal.AddCommand<DogCommand>("dog");
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
        public void Should_Register_Commands_When_Configuring_Application()
        {
            // Given
            var registrar = new FakeTypeRegistrar();
            var app = new CommandApp(registrar);

            // When
            app.Configure(config =>
            {
                config.PropagateExceptions();
                config.AddCommand<GenericCommand<FooCommandSettings>>("foo");
                config.AddBranch<AnimalSettings>("animal", animal =>
                {
                    animal.AddCommand<DogCommand>("dog");
                    animal.AddCommand<HorseCommand>("horse");
                });
            });

            // Then
            registrar.Registrations.ContainsKey(typeof(ICommand)).ShouldBeTrue();
            registrar.Registrations[typeof(ICommand)].Count.ShouldBe(3);
            registrar.Registrations[typeof(ICommand)].ShouldContain(typeof(GenericCommand<FooCommandSettings>));
            registrar.Registrations[typeof(ICommand)].ShouldContain(typeof(DogCommand));
            registrar.Registrations[typeof(ICommand)].ShouldContain(typeof(HorseCommand));
        }

        [Fact]
        public void Should_Register_Default_Command_When_Configuring_Application()
        {
            // Given
            var registrar = new FakeTypeRegistrar();
            var app = new CommandApp<DogCommand>(registrar);

            // When
            app.Configure(config =>
            {
                config.PropagateExceptions();
            });

            // Then
            registrar.Registrations.ContainsKey(typeof(ICommand)).ShouldBeTrue();
            registrar.Registrations.ContainsKey(typeof(DogSettings));
            registrar.Registrations[typeof(ICommand)].Count.ShouldBe(1);
            registrar.Registrations[typeof(ICommand)].ShouldContain(typeof(DogCommand));
        }

        [Fact]
        public void Should_Register_Default_Command_Settings_When_Configuring_Application()
        {
            // Given
            var registrar = new FakeTypeRegistrar();
            var app = new CommandApp<DogCommand>(registrar);

            // When
            app.Configure(config =>
            {
                config.PropagateExceptions();
            });

            // Then
            registrar.Registrations.ContainsKey(typeof(DogSettings));
            registrar.Registrations[typeof(DogSettings)].Count.ShouldBe(1);
            registrar.Registrations[typeof(DogSettings)].ShouldContain(typeof(DogSettings));
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
                config.PropagateExceptions();
                config.AddBranch<AnimalSettings>("animal", animal =>
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

        [Fact]
        public void Should_Throw_When_Encountering_Unknown_Option_In_Strict_Mode()
        {
            // Given
            var registrar = new FakeTypeRegistrar();
            var app = new CommandApp(registrar);
            app.Configure(config =>
            {
                config.PropagateExceptions();
                config.UseStrictParsing();
                config.AddCommand<DogCommand>("dog");
            });

            // When
            var result = Record.Exception(() => app.Run(new[] { "dog", "--foo" }));

            // Then
            result.ShouldBeOfType<ParseException>().And(ex =>
            {
                ex.Message.ShouldBe("Unknown option 'foo'.");
            });
        }

        [Fact]
        public void Should_Add_Unknown_Option_To_Remaining_Arguments_In_Relaxed_Mode()
        {
            // Given
            var capturedContext = default(CommandContext);

            var resolver = new FakeTypeResolver();
            var command = new InterceptingCommand<DogSettings>((context, _) => { capturedContext = context; });
            resolver.Register(new DogSettings());
            resolver.Register(command);

            var registrar = new FakeTypeRegistrar(resolver);
            var app = new CommandApp(registrar);

            app.Configure(config =>
            {
                config.PropagateExceptions();
                config.AddBranch<AnimalSettings>("animal", animal =>
                {
                    animal.AddCommand<InterceptingCommand<DogSettings>>("dog");
                });
            });

            // When
            var result = app.Run(new[] { "animal", "4", "dog", "12", "--foo", "bar" });

            // Then
            capturedContext.ShouldNotBeNull();
            capturedContext.Remaining.Parsed.Count.ShouldBe(1);
            capturedContext.ShouldHaveRemainingArgument("foo", values: new[] { "bar" });
        }

        [Fact]
        public void Should_Add_Unknown_Boolean_Option_To_Remaining_Arguments_In_Relaxed_Mode()
        {
            // Given
            var capturedContext = default(CommandContext);

            var resolver = new FakeTypeResolver();
            var command = new InterceptingCommand<DogSettings>((context, _) => { capturedContext = context; });
            resolver.Register(new DogSettings());
            resolver.Register(command);

            var registrar = new FakeTypeRegistrar(resolver);
            var app = new CommandApp(registrar);

            app.Configure(config =>
            {
                config.PropagateExceptions();
                config.AddBranch<AnimalSettings>("animal", animal =>
                {
                    animal.AddCommand<InterceptingCommand<DogSettings>>("dog");
                });
            });

            // When
            var result = app.Run(new[] { "animal", "4", "dog", "12", "--foo" });

            // Then
            capturedContext.ShouldNotBeNull();
            capturedContext.Remaining.Parsed.Count.ShouldBe(1);
            capturedContext.ShouldHaveRemainingArgument("foo", values: new[] { (string)null });
        }

        public sealed class Remaining_Arguments
        {
            [Fact]
            public void Should_Register_Remaining_Parsed_Arguments_With_Context()
            {
                // Given
                var capturedContext = default(CommandContext);

                var resolver = new FakeTypeResolver();
                var command = new InterceptingCommand<DogSettings>((context, _) => { capturedContext = context; });
                resolver.Register(new DogSettings());
                resolver.Register(command);

                var app = new CommandApp(new FakeTypeRegistrar(resolver));
                app.Configure(config =>
                {
                    config.PropagateExceptions();
                    config.AddBranch<AnimalSettings>("animal", animal =>
                    {
                        animal.AddCommand<InterceptingCommand<DogSettings>>("dog");
                    });
                });

                // When
                app.Run(new[] { "animal", "4", "dog", "12", "--", "--foo", "bar", "--foo", "baz", "-bar", "\"baz\"", "qux" });

                // Then
                capturedContext.Remaining.Parsed.Count.ShouldBe(4);
                capturedContext.ShouldHaveRemainingArgument("foo", values: new[] { "bar", "baz" });
                capturedContext.ShouldHaveRemainingArgument("b", values: new[] { (string)null });
                capturedContext.ShouldHaveRemainingArgument("a", values: new[] { (string)null });
                capturedContext.ShouldHaveRemainingArgument("r", values: new[] { (string)null });
            }

            [Fact]
            public void Should_Register_Remaining_Raw_Arguments_With_Context()
            {
                // Given
                var capturedContext = default(CommandContext);

                var resolver = new FakeTypeResolver();
                var command = new InterceptingCommand<DogSettings>((context, _) => { capturedContext = context; });
                resolver.Register(new DogSettings());
                resolver.Register(command);

                var app = new CommandApp(new FakeTypeRegistrar(resolver));
                app.Configure(config =>
                {
                    config.PropagateExceptions();
                    config.AddBranch<AnimalSettings>("animal", animal =>
                    {
                        animal.AddCommand<InterceptingCommand<DogSettings>>("dog");
                    });
                });

                // When
                app.Run(new[] { "animal", "4", "dog", "12", "--", "--foo", "bar", "-bar", "\"baz\"", "qux" });

                // Then
                capturedContext.Remaining.Raw.Count.ShouldBe(5);
                capturedContext.Remaining.Raw[0].ShouldBe("--foo");
                capturedContext.Remaining.Raw[1].ShouldBe("bar");
                capturedContext.Remaining.Raw[2].ShouldBe("-bar");
                capturedContext.Remaining.Raw[3].ShouldBe("\"baz\"");
                capturedContext.Remaining.Raw[4].ShouldBe("qux");
            }
        }

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

        public sealed class Exception_Handling
        {
            [Fact]
            public void Should_Not_Propagate_Runtime_Exceptions_If_Not_Explicitly_Told_To_Do_So()
            {
                // Given
                var app = new CommandApp(new FakeTypeRegistrar());
                app.Configure(config =>
                {
                    config.AddBranch<AnimalSettings>("animal", animal =>
                    {
                        animal.AddCommand<DogCommand>("dog");
                        animal.AddCommand<HorseCommand>("horse");
                    });
                });

                // When
                var result = app.Run(new[] { "animal", "4", "dog", "101", "--name", "Rufus" });

                // Then
                result.ShouldBe(-1);
            }

            [Fact]
            public void Should_Not_Propagate_Exceptions_If_Not_Explicitly_Told_To_Do_So()
            {
                // Given
                var app = new CommandApp(new FakeTypeRegistrar());
                app.Configure(config =>
                {
                    config.AddCommand<ThrowingCommand>("throw");
                });

                // When
                var result = app.Run(new[] { "throw" });

                // Then
                result.ShouldBe(-1);
            }
        }
    }
}
