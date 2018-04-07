using System;
using System.Text;
using Shouldly;
using Spectre.CommandLine.Internal;
using Spectre.CommandLine.Internal.Configuration;
using Spectre.CommandLine.Internal.Modelling;
using Spectre.CommandLine.Internal.Rendering;
using Spectre.CommandLine.Tests.Data;
using Spectre.CommandLine.Tests.Data.Settings;
using Spectre.CommandLine.Tests.Fakes;
using Spectre.CommandLine.Tests.Utilities;
using Xunit;

namespace Spectre.CommandLine.Tests.Unit.Internal
{
    public sealed class HelpWriterTests
    {
        [Theory]
        [EmbeddedResourceData("Spectre.CommandLine.Tests/Data/Resources/Help/Root")]
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
        [EmbeddedResourceData("Spectre.CommandLine.Tests/Data/Resources/Help/Command")]
        public void Should_Output_Command_Correctly(string expected)
        {
            // Given
            var configurator = new Configurator(new FakeTypeRegistrar());
            configurator.SetApplicationName("myapp");
            configurator.AddCommand<CatSettings>("cat", animal =>
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
        [EmbeddedResourceData("Spectre.CommandLine.Tests/Data/Resources/Help/Leaf")]
        public void Should_Output_Leaf_Correctly(string expected)
        {
            // Given
            var configurator = new Configurator(new FakeTypeRegistrar());
            configurator.SetApplicationName("myapp");
            configurator.AddCommand<CatSettings>("cat", animal =>
            {
                animal.SetDescription("Contains settings for a cat.");
                animal.AddCommand<LionCommand>("lion");
            });

            // When
            var result = Fixture.Write(configurator, model => model.Commands[0].Children[0]);

            // Then
            result.ShouldBe(expected);
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
