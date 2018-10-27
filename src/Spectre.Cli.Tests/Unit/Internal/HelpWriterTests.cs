using System;
using System.Text;
using Shouldly;
using Spectre.Cli.Internal;
using Spectre.Cli.Internal.Configuration;
using Spectre.Cli.Internal.Modelling;
using Spectre.Cli.Internal.Rendering;
using Spectre.Cli.Tests.Data;
using Spectre.Cli.Tests.Data.Settings;
using Spectre.Cli.Tests.Fakes;
using Spectre.Cli.Tests.Utilities;
using Xunit;

namespace Spectre.Cli.Tests.Unit.Internal
{
    public sealed class HelpWriterTests
    {
        [Theory]
        [EmbeddedResourceData("Spectre.Cli.Tests/Data/Resources/Help/Root")]
        public void Should_Output_Root_Correctly(string expected)
        {
            // Given
            var configurator = new Configurator(new FakeTypeRegistrar());
            configurator.SetApplicationName("myapp");
            configurator.AddCommand<DogCommand>("dog");
            configurator.AddCommand<HorseCommand>("horse");
            configurator.AddCommand<GiraffeCommand>("giraffe");

            // When
            var result = Fixture.Write(configurator);

            // Then
            result.ShouldBe(expected);
        }

        [Theory]
        [EmbeddedResourceData("Spectre.Cli.Tests/Data/Resources/Help/Command")]
        public void Should_Output_Command_Correctly(string expected)
        {
            // Given
            var configurator = new Configurator(new FakeTypeRegistrar());
            configurator.SetApplicationName("myapp");
            configurator.AddBranch<CatSettings>("cat", animal =>
            {
                animal.SetDescription("Contains settings for a cat.");
                animal.AddCommand<LionCommand>("lion");
            });

            // When
            var result = Fixture.Write(configurator, model => model.Commands[0]);

            // Then
            result.ShouldBe(expected);
        }

        [Theory]
        [EmbeddedResourceData("Spectre.Cli.Tests/Data/Resources/Help/Leaf")]
        public void Should_Output_Leaf_Correctly(string expected)
        {
            // Given
            var configurator = new Configurator(new FakeTypeRegistrar());
            configurator.SetApplicationName("myapp");
            configurator.AddBranch<CatSettings>("cat", animal =>
            {
                animal.SetDescription("Contains settings for a cat.");
                animal.AddCommand<LionCommand>("lion");
            });

            // When
            var result = Fixture.Write(configurator, model => model.Commands[0].Children[0]);

            // Then
            result.ShouldBe(expected);
        }

        [Theory]
        [EmbeddedResourceData("Spectre.Cli.Tests/Data/Resources/Help/Default")]
        public void Should_Output_Default_Command_Correctly(string expected)
        {
            // Given
            var configurator = new Configurator(new FakeTypeRegistrar());
            configurator.SetDefaultCommand<LionCommand>();
            configurator.SetApplicationName("myapp");

            // When
            var result = Fixture.Write(configurator, model => model.DefaultCommand);

            // Then
            result.ShouldBe(expected);
        }

        public sealed class Examples
        {
            [Theory]
            [EmbeddedResourceData("Spectre.Cli.Tests/Data/Resources/Help/Root_Examples_DefinedOnRoot")]
            public void Should_Output_Root_Examples_Defined_On_Root(string expected)
            {
                // Given
                var configurator = new Configurator(new FakeTypeRegistrar());
                configurator.SetApplicationName("myapp");
                configurator.AddExample(new[] { "dog", "--name", "Rufus", "--age", "12", "--good-boy" });
                configurator.AddExample(new[] { "horse", "--name", "Brutus" });
                configurator.AddCommand<DogCommand>("dog");
                configurator.AddCommand<HorseCommand>("horse");

                // When
                var result = Fixture.Write(configurator);

                // Then
                result.ShouldBe(expected);
            }

            [Theory]
            [EmbeddedResourceData("Spectre.Cli.Tests/Data/Resources/Help/Root_Examples_DefinedOnChildren")]
            public void Should_Output_Root_Examples_Defined_On_Direct_Children_If_Root_Have_No_Examples(string expected)
            {
                // Given
                var configurator = new Configurator(new FakeTypeRegistrar());
                configurator.SetApplicationName("myapp");
                configurator.AddCommand<DogCommand>("dog")
                    .WithExample(new[] { "dog", "--name", "Rufus", "--age", "12", "--good-boy" });
                configurator.AddCommand<HorseCommand>("horse")
                    .WithExample(new[] { "horse", "--name", "Brutus" });

                // When
                var result = Fixture.Write(configurator);

                // Then
                result.ShouldBe(expected);
            }

            [Theory]
            [EmbeddedResourceData("Spectre.Cli.Tests/Data/Resources/Help/Root_Examples_DefinedOnLeaves")]
            public void Should_Output_Root_Examples_Defined_On_Leaves_If_No_Other_Examples_Are_Found(string expected)
            {
                // Given
                var configurator = new Configurator(new FakeTypeRegistrar());
                configurator.SetApplicationName("myapp");
                configurator.AddBranch<AnimalSettings>("animal", animal =>
                {
                    animal.SetDescription("The animal command.");

                    animal.AddCommand<DogCommand>("dog")
                        .WithExample(new[] { "animal", "dog", "--name", "Rufus", "--age", "12", "--good-boy" });
                    animal.AddCommand<HorseCommand>("horse")
                        .WithExample(new[] { "animal", "horse", "--name", "Brutus" });
                });

                // When
                var result = Fixture.Write(configurator);

                // Then
                result.ShouldBe(expected);
            }

            [Theory]
            [EmbeddedResourceData("Spectre.Cli.Tests/Data/Resources/Help/Command_Examples_DefinedOnCommand")]
            public void Should_Only_Output_Command_Examples_Defined_On_Command(string expected)
            {
                // Given
                var configurator = new Configurator(new FakeTypeRegistrar());
                configurator.SetApplicationName("myapp");
                configurator.AddBranch<AnimalSettings>("animal", animal =>
                {
                    animal.SetDescription("The animal command.");
                    animal.AddExample(new[] { "animal", "--help" });

                    animal.AddCommand<DogCommand>("dog")
                        .WithExample(new[] { "animal", "dog", "--name", "Rufus", "--age", "12", "--good-boy" });
                    animal.AddCommand<HorseCommand>("horse")
                        .WithExample(new[] { "animal", "horse", "--name", "Brutus" });
                });

                // When
                var result = Fixture.Write(configurator, c => c.Commands[0]);

                // Then
                result.ShouldBe(expected);
            }
        }

        internal static class Fixture
        {
            public static string Write(Configurator configurator)
            {
                var model = CommandModelBuilder.Build(configurator);
                var output = HelpWriter.Write(model);
                return Render(output);
            }

            public static string Write(Configurator configurator, Func<CommandModel, CommandInfo> command)
            {
                var model = CommandModelBuilder.Build(configurator);
                var output = HelpWriter.WriteCommand(model, command(model));
                return Render(output);
            }

            private static string Render(IRenderable renderable)
            {
                var builder = new StringBuilder();
                var renderer = new StringRenderer(builder);
                renderable.Render(renderer);
                return builder.ToString().NormalizeLineEndings().Trim();
            }
        }
    }
}
