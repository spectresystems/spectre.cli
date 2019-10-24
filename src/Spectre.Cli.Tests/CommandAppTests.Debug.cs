using System.Collections.Generic;
using Shouldly;
using Spectre.Cli.Testing;
using Spectre.Cli.Testing.Data.Commands;
using Spectre.Cli.Testing.Data.Settings;
using Xunit;

namespace Spectre.Cli.Tests
{
    public sealed partial class CommandAppTests
    {
        public sealed class Debug
        {
            /// <summary>
            /// https://github.com/spectresystems/spectre.cli/wiki/Test-cases#test-case-1
            /// </summary>
            [Theory]
            [EmbeddedResourceData("Spectre.Cli.Tests/Properties/Resources/Parsing/case1.xml")]
            public void Should_Parse_Correct_Tree_For_Case_1(string expected)
            {
                // Given
                var fixture = new CommandAppFixture();
                fixture.Configure(config =>
                {
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
                var (_, output) = fixture.Run(
                    "__debug", "animal", "--alive", "mammal",
                    "--name", "Rufus", "dog", "12", "--good-boy");

                // Then
                output.ShouldBe(expected);
            }

            /// <summary>
            /// https://github.com/spectresystems/spectre.cli/wiki/Test-cases#test-case-2
            /// </summary>
            [Theory]
            [EmbeddedResourceData("Spectre.Cli.Tests/Properties/Resources/Parsing/case2.xml")]
            public void Should_Parse_Correct_Tree_For_Case_2(string expected)
            {
                // Given
                var fixture = new CommandAppFixture();
                fixture.Configure(config =>
                {
                    config.AddCommand<DogCommand>("dog");
                });

                // When
                var (_, output) = fixture.Run(
                    "__debug", "dog", "12", "4",
                    "--good-boy", "--name", "Rufus", "--alive");

                // Then
                output.ShouldBe(expected);
            }

            /// <summary>
            /// https://github.com/spectresystems/spectre.cli/wiki/Test-cases#test-case-3
            /// </summary>
            [Theory]
            [EmbeddedResourceData("Spectre.Cli.Tests/Properties/Resources/Parsing/case3.xml")]
            public void Should_Parse_Correct_Tree_For_Case_3(string expected)
            {
                // Given
                var fixture = new CommandAppFixture();
                fixture.Configure(config =>
                {
                    config.AddBranch<AnimalSettings>("animal", animal =>
                    {
                        animal.AddCommand<DogCommand>("dog");
                        animal.AddCommand<HorseCommand>("horse");
                    });
                });

                // When
                var (_, output) = fixture.Run(
                    "__debug", "animal", "dog", "12",
                    "--good-boy", "--name", "Rufus");

                // Then
                output.ShouldBe(expected);
            }

            /// <summary>
            /// https://github.com/spectresystems/spectre.cli/wiki/Test-cases#test-case-4
            /// </summary>
            [Theory]
            [EmbeddedResourceData("Spectre.Cli.Tests/Properties/Resources/Parsing/case4.xml")]
            public void Should_Parse_Correct_Tree_For_Case_4(string expected)
            {
                // Given
                var fixture = new CommandAppFixture();
                fixture.Configure(config =>
                {
                    config.AddBranch<AnimalSettings>("animal", animal =>
                    {
                        animal.AddCommand<DogCommand>("dog");
                    });
                });

                // When
                var (_, output) = fixture.Run(
                    "__debug", "animal", "4", "dog", "12",
                    "--good-boy", "--name", "Rufus");

                // Then
                output.ShouldBe(expected);
            }

            /// <summary>
            /// https://github.com/spectresystems/spectre.cli/wiki/Test-cases#test-case-5
            /// </summary>
            [Theory]
            [EmbeddedResourceData("Spectre.Cli.Tests/Properties/Resources/Parsing/case5.xml")]
            public void Should_Parse_Correct_Tree_For_Case_5(string expected)
            {
                // Given
                var fixture = new CommandAppFixture();
                fixture.Configure(config =>
                {
                    config.AddCommand<OptionVectorCommand>("cmd");
                });

                // When
                var (_, output) = fixture.Run(
                    "__debug", "cmd", "--foo", "red", "--bar",
                    "4", "--foo", "blue");

                // Then
                output.ShouldBe(expected);
            }

            [Theory]
            [EmbeddedResourceData("Spectre.Cli.Tests/Properties/Resources/Parsing/default1.xml")]
            [EmbeddedResourceData("Spectre.Cli.Tests/Properties/Resources/Parsing/default2.xml", "--good-boy")]
            [EmbeddedResourceData("Spectre.Cli.Tests/Properties/Resources/Parsing/default3.xml", "--help")]
            [EmbeddedResourceData("Spectre.Cli.Tests/Properties/Resources/Parsing/default4.xml", "4", "12", "--good-boy")]
            public void Should_Use_Default_Command_If_No_Command_Was_Specified(string expected, params string[] args)
            {
                // Given
                var fixture = new CommandAppFixture();
                fixture.WithDefaultCommand<DogCommand>();
                fixture.Configure(config =>
                {
                    config.AddCommand<CatCommand>("cat");
                });

                var parameters = new List<string>();
                parameters.Add("__debug");
                parameters.AddRange(args);

                // When
                var (_, output) = fixture.Run(parameters.ToArray());

                // Then
                output.ShouldBe(expected);
            }
        }
    }
}
