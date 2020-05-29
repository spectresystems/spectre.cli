using Shouldly;
using Spectre.Cli.Testing;
using Spectre.Cli.Testing.Data.Commands;
using Spectre.Cli.Testing.Data.Settings;
using Xunit;

namespace Spectre.Cli.Tests
{
    public sealed partial class CommandAppTests
    {
        public sealed class Xml
        {
            /// <summary>
            /// https://github.com/spectresystems/spectre.cli/wiki/Test-cases#test-case-1
            /// </summary>
            [Theory]
            [EmbeddedResourceData("Spectre.Cli.Tests/Properties/Resources/Models/case1.xml")]
            public void Should_Dump_Correct_Model_For_Case_1(string expected)
            {
                // Given
                var fixture = new CommandAppFixture();
                fixture.Configure(config =>
                {
                    config.EnableXmlDoc();
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
                var (_, output) = fixture.Run("@xmldoc");

                // Then
                output.ShouldBe(expected);
            }

            /// <summary>
            /// https://github.com/spectresystems/spectre.cli/wiki/Test-cases#test-case-2
            /// </summary>
            [Theory]
            [EmbeddedResourceData("Spectre.Cli.Tests/Properties/Resources/Models/case2.xml")]
            public void Should_Dump_Correct_Model_For_Case_2(string expected)
            {
                // Given
                var fixture = new CommandAppFixture();
                fixture.Configure(config =>
                {
                    config.EnableXmlDoc();
                    config.AddCommand<DogCommand>("dog");
                });

                // When
                var (_, output) = fixture.Run("@xmldoc");

                // Then
                output.ShouldBe(expected);
            }

            /// <summary>
            /// https://github.com/spectresystems/spectre.cli/wiki/Test-cases#test-case-3
            /// </summary>
            [Theory]
            [EmbeddedResourceData("Spectre.Cli.Tests/Properties/Resources/Models/case3.xml")]
            public void Should_Dump_Correct_Model_For_Case_3(string expected)
            {
                // Given
                var fixture = new CommandAppFixture();
                fixture.Configure(config =>
                {
                    config.EnableXmlDoc();
                    config.AddBranch<AnimalSettings>("animal", animal =>
                    {
                        animal.AddCommand<DogCommand>("dog");
                        animal.AddCommand<HorseCommand>("horse");
                    });
                });

                // When
                var (_, output) = fixture.Run("@xmldoc");

                // Then
                output.ShouldBe(expected);
            }

            /// <summary>
            /// https://github.com/spectresystems/spectre.cli/wiki/Test-cases#test-case-4
            /// </summary>
            [Theory]
            [EmbeddedResourceData("Spectre.Cli.Tests/Properties/Resources/Models/case4.xml")]
            public void Should_Dump_Correct_Model_For_Case_4(string expected)
            {
                // Given
                var fixture = new CommandAppFixture();
                fixture.Configure(config =>
                {
                    config.EnableXmlDoc();
                    config.AddBranch<AnimalSettings>("animal", animal =>
                    {
                        animal.AddCommand<DogCommand>("dog");
                    });
                });

                // When
                var (_, output) = fixture.Run("@xmldoc");

                // Then
                output.ShouldBe(expected);
            }

            /// <summary>
            /// https://github.com/spectresystems/spectre.cli/wiki/Test-cases#test-case-5
            /// </summary>
            [Theory]
            [EmbeddedResourceData("Spectre.Cli.Tests/Properties/Resources/Models/case5.xml")]
            public void Should_Dump_Correct_Model_For_Case_5(string expected)
            {
                // Given
                var fixture = new CommandAppFixture();
                fixture.Configure(config =>
                {
                    config.EnableXmlDoc();
                    config.AddCommand<OptionVectorCommand>("cmd");
                });

                // When
                var (_, output) = fixture.Run("@xmldoc");

                // Then
                output.ShouldBe(expected);
            }
        }
    }
}
