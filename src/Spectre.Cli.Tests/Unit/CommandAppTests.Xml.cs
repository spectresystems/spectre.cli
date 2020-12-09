using System.Threading.Tasks;
using Shouldly;
using Spectre.Cli.Testing;
using Spectre.Cli.Testing.Data.Commands;
using Spectre.Cli.Testing.Data.Settings;
using VerifyXunit;
using Xunit;

namespace Spectre.Cli.Tests
{
    public sealed partial class CommandAppTests
    {
        [UsesVerify]
        public sealed class Xml
        {
            /// <summary>
            /// https://github.com/spectresystems/spectre.cli/wiki/Test-cases#test-case-1
            /// </summary>
            [Fact]
            public Task Should_Dump_Correct_Model_For_Case_1()
            {
                // Given
                var fixture = new CommandAppFixture();
                fixture.Configure(config =>
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
                var (_, output, _, _) = fixture.Run(Constants.XmlDocCommand);

                // Then
                return Verifier.Verify(output);
            }

            /// <summary>
            /// https://github.com/spectresystems/spectre.cli/wiki/Test-cases#test-case-2
            /// </summary>
            [Theory]
            [EmbeddedResourceData("Spectre.Cli.Tests/Resources/Models/case2.xml")]
            public void Should_Dump_Correct_Model_For_Case_2(string expected)
            {
                // Given
                var fixture = new CommandAppFixture();
                fixture.Configure(config =>
                {
                    config.AddCommand<DogCommand>("dog");
                });

                // When
                var (_, output, _, _) = fixture.Run(Constants.XmlDocCommand);

                // Then
                output.ShouldBe(expected);
            }

            /// <summary>
            /// https://github.com/spectresystems/spectre.cli/wiki/Test-cases#test-case-3
            /// </summary>
            [Theory]
            [EmbeddedResourceData("Spectre.Cli.Tests/Resources/Models/case3.xml")]
            public void Should_Dump_Correct_Model_For_Case_3(string expected)
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
                var (_, output, _, _) = fixture.Run(Constants.XmlDocCommand);

                // Then
                output.ShouldBe(expected);
            }

            /// <summary>
            /// https://github.com/spectresystems/spectre.cli/wiki/Test-cases#test-case-4
            /// </summary>
            [Theory]
            [EmbeddedResourceData("Spectre.Cli.Tests/Resources/Models/case4.xml")]
            public void Should_Dump_Correct_Model_For_Case_4(string expected)
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
                var (_, output, _, _) = fixture.Run(Constants.XmlDocCommand);

                // Then
                output.ShouldBe(expected);
            }

            /// <summary>
            /// https://github.com/spectresystems/spectre.cli/wiki/Test-cases#test-case-5
            /// </summary>
            [Theory]
            [EmbeddedResourceData("Spectre.Cli.Tests/Resources/Models/case5.xml")]
            public void Should_Dump_Correct_Model_For_Case_5(string expected)
            {
                // Given
                var fixture = new CommandAppFixture();
                fixture.Configure(config =>
                {
                    config.AddCommand<OptionVectorCommand>("cmd");
                });

                // When
                var (_, output, _, _) = fixture.Run(Constants.XmlDocCommand);

                // Then
                output.ShouldBe(expected);
            }

            [Theory]
            [EmbeddedResourceData("Spectre.Cli.Tests/Resources/Xml/default_command.xml")]
            public void Should_Dump_Correct_Model_For_Model_With_Default_Command(string expected)
            {
                // Given
                var fixture = new CommandAppFixture().WithDefaultCommand<DogCommand>();
                fixture.Configure(config =>
                {
                    config.AddCommand<HorseCommand>("horse");
                });

                // When
                var (_, output, _, _) = fixture.Run(Constants.XmlDocCommand);

                // Then
                output.ShouldBe(expected);
            }
        }
    }
}
