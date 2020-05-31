using Shouldly;
using Spectre.Cli.Testing;
using Spectre.Cli.Testing.Data.Commands;
using Spectre.Cli.Testing.Data.Settings;
using Xunit;

namespace Spectre.Cli.Tests
{
    public sealed partial class CommandAppTests
    {
        public class Help
        {
            [Theory]
            [EmbeddedResourceData("Spectre.Cli.Tests/Properties/Resources/Help/Root")]
            public void Should_Output_Root_Correctly(string expected)
            {
                // Given
                var fixture = new CommandAppFixture();
                fixture.Configure(configurator =>
                {
                    configurator.SetApplicationName("myapp");
                    configurator.AddCommand<DogCommand>("dog");
                    configurator.AddCommand<HorseCommand>("horse");
                    configurator.AddCommand<GiraffeCommand>("giraffe");
                });

                // When
                var (_, output, _, _) = fixture.Run("--help");

                // Then
                output.ShouldBe(expected);
            }

            [Theory]
            [EmbeddedResourceData("Spectre.Cli.Tests/Properties/Resources/Help/Hidden")]
            public void Should_Skip_Hidden_Commands(string expected)
            {
                // Given
                var fixture = new CommandAppFixture();
                fixture.Configure(configurator =>
                {
                    configurator.SetApplicationName("myapp");
                    configurator.AddCommand<DogCommand>("dog");
                    configurator.AddCommand<HorseCommand>("horse");
                    configurator.AddCommand<GiraffeCommand>("giraffe").IsHidden();
                });

                // When
                var (_, output, _, _) = fixture.Run("--help");

                // Then
                output.ShouldBe(expected);
            }

            [Theory]
            [EmbeddedResourceData("Spectre.Cli.Tests/Properties/Resources/Help/Command")]
            public void Should_Output_Command_Correctly(string expected)
            {
                // Given
                var fixture = new CommandAppFixture();
                fixture.Configure(configurator =>
                {
                    configurator.SetApplicationName("myapp");
                    configurator.AddBranch<CatSettings>("cat", animal =>
                    {
                        animal.SetDescription("Contains settings for a cat.");
                        animal.AddCommand<LionCommand>("lion");
                    });
                });

                // When
                var (_, output, _, _) = fixture.Run("cat", "--help");

                // Then
                output.ShouldBe(expected);
            }

            [Theory]
            [EmbeddedResourceData("Spectre.Cli.Tests/Properties/Resources/Help/Leaf")]
            public void Should_Output_Leaf_Correctly(string expected)
            {
                // Given
                var fixture = new CommandAppFixture();
                fixture.Configure(configurator =>
                {
                    configurator.SetApplicationName("myapp");
                    configurator.AddBranch<CatSettings>("cat", animal =>
                    {
                        animal.SetDescription("Contains settings for a cat.");
                        animal.AddCommand<LionCommand>("lion");
                    });
                });

                // When
                var (_, output, _, _) = fixture.Run("cat", "lion", "--help");

                // Then
                output.ShouldBe(expected);
            }

            [Theory]
            [EmbeddedResourceData("Spectre.Cli.Tests/Properties/Resources/Help/Default")]
            public void Should_Output_Default_Command_Correctly(string expected)
            {
                // Given
                var fixture = new CommandAppFixture();
                fixture.WithDefaultCommand<LionCommand>();
                fixture.Configure(configurator =>
                {
                    configurator.SetApplicationName("myapp");
                });

                // When
                var (_, output, _, _) = fixture.Run("--help");

                // Then
                output.ShouldBe(expected);
            }

            [Theory]
            [EmbeddedResourceData("Spectre.Cli.Tests/Properties/Resources/Help/Root_Examples_DefinedOnRoot")]
            public void Should_Output_Root_Examples_Defined_On_Root(string expected)
            {
                // Given
                var fixture = new CommandAppFixture();
                fixture.Configure(configurator =>
                {
                    configurator.SetApplicationName("myapp");
                    configurator.AddExample(new[] { "dog", "--name", "Rufus", "--age", "12", "--good-boy" });
                    configurator.AddExample(new[] { "horse", "--name", "Brutus" });
                    configurator.AddCommand<DogCommand>("dog");
                    configurator.AddCommand<HorseCommand>("horse");
                });

                // When
                var (_, output, _, _) = fixture.Run("--help");

                // Then
                output.ShouldBe(expected);
            }

            [Theory]
            [EmbeddedResourceData("Spectre.Cli.Tests/Properties/Resources/Help/Root_Examples_DefinedOnChildren")]
            public void Should_Output_Root_Examples_Defined_On_Direct_Children_If_Root_Have_No_Examples(string expected)
            {
                // Given
                var fixture = new CommandAppFixture();
                fixture.Configure(configurator =>
                {
                    configurator.SetApplicationName("myapp");
                    configurator.AddCommand<DogCommand>("dog")
                        .WithExample(new[] { "dog", "--name", "Rufus", "--age", "12", "--good-boy" });
                    configurator.AddCommand<HorseCommand>("horse")
                        .WithExample(new[] { "horse", "--name", "Brutus" });
                });

                // When
                var (_, output, _, _) = fixture.Run("--help");

                // Then
                output.ShouldBe(expected);
            }

            [Theory]
            [EmbeddedResourceData("Spectre.Cli.Tests/Properties/Resources/Help/Root_Examples_DefinedOnLeaves")]
            public void Should_Output_Root_Examples_Defined_On_Leaves_If_No_Other_Examples_Are_Found(string expected)
            {
                // Given
                var fixture = new CommandAppFixture();
                fixture.Configure(configurator =>
                {
                    configurator.SetApplicationName("myapp");
                    configurator.AddBranch<AnimalSettings>("animal", animal =>
                    {
                        animal.SetDescription("The animal command.");
                        animal.AddCommand<DogCommand>("dog")
                            .WithExample(new[] { "animal", "dog", "--name", "Rufus", "--age", "12", "--good-boy" });
                        animal.AddCommand<HorseCommand>("horse")
                            .WithExample(new[] { "animal", "horse", "--name", "Brutus" });
                    });
                });

                // When
                var (_, output, _, _) = fixture.Run("--help");

                // Then
                output.ShouldBe(expected);
            }

            [Theory]
            [EmbeddedResourceData("Spectre.Cli.Tests/Properties/Resources/Help/Command_Examples_DefinedOnCommand")]
            public void Should_Only_Output_Command_Examples_Defined_On_Command(string expected)
            {
                // Given
                var fixture = new CommandAppFixture();
                fixture.Configure(configurator =>
                {
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
                });

                // When
                var (_, output, _, _) = fixture.Run("animal", "--help");

                // Then
                output.ShouldBe(expected);
            }

            [Theory]
            [EmbeddedResourceData("Spectre.Cli.Tests/Properties/Resources/Help/Default_Examples_DefinedOnRoot")]
            public void Should_Output_Root_Examples_If_Default_Command_Is_Specified(string expected)
            {
                // Given
                var fixture = new CommandAppFixture();
                fixture.WithDefaultCommand<LionCommand>();
                fixture.Configure(configurator =>
                {
                    configurator.SetApplicationName("myapp");
                    configurator.AddExample(new[] { "12", "-c", "3" });
                });

                // When
                var (_, output, _, _) = fixture.Run("--help");

                // Then
                output.ShouldBe(expected);
            }
        }
    }
}
