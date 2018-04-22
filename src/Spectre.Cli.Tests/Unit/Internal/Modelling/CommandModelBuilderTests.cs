using Shouldly;
using Spectre.Cli.Internal.Configuration;
using Spectre.Cli.Internal.Exceptions;
using Spectre.Cli.Internal.Modelling;
using Spectre.Cli.Tests.Data;
using Spectre.Cli.Tests.Data.Settings;
using Xunit;

namespace Spectre.Cli.Tests.Unit.Internal.Modelling
{
    public sealed class CommandModelBuilderTests
    {
        [Fact]
        public void Should_Set_Parents_Correctly()
        {
            // Given
            var configurator = new Configurator(null);
            configurator.AddBranch<AnimalSettings>("animal", animal =>
            {
                animal.AddBranch<MammalSettings>("mammal", mammal =>
                {
                    mammal.AddCommand<DogCommand>("dog");
                    mammal.AddCommand<HorseCommand>("horse");
                });
            });

            // When
            var model = CommandModelBuilder.Build(configurator);

            // Then
            model.Commands.Count.ShouldBe(1);
            model.Commands[0].As(animal =>
            {
                animal.Parent.ShouldBeNull();
                animal.Children.Count.ShouldBe(1);
                animal.Children[0].As(mammal =>
                {
                    mammal.Parent.ShouldBe(animal);
                    mammal.Children.Count.ShouldBe(2);
                    mammal.Children[0].As(dog => dog.Parent.ShouldBe(mammal));
                    mammal.Children[1].As(horse => horse.Parent.ShouldBe(mammal));
                });
            });
        }

        [Fact]
        public void Should_Set_Descriptions_For_Branches()
        {
            // Given
            var configurator = new Configurator(null);
            configurator.AddBranch<AnimalSettings>("animal", animal =>
            {
                animal.SetDescription("An animal");
                animal.AddBranch<MammalSettings>("mammal", mammal =>
                {
                    mammal.SetDescription("A mammal");
                    mammal.AddCommand<DogCommand>("dog");
                    mammal.AddCommand<HorseCommand>("horse");
                });
            });

            // When
            var model = CommandModelBuilder.Build(configurator);

            // Then
            model.Commands[0].As(animal =>
            {
                animal.Description.ShouldBe("An animal");
                animal.Children[0].As(mammal =>
                {
                    mammal.Description.ShouldBe("A mammal");
                });
            });
        }

        [Fact]
        public void Should_Set_TypeConverter_For_Options()
        {
            // Given
            var configurator = new Configurator(null);
            configurator.AddCommand<CatCommand>("cat");

            // When
            var model = CommandModelBuilder.Build(configurator);

            // Then
            model.Commands[0]
                .GetOption(option => option.LongName == "agility")
                .Converter.ConverterTypeName
                .ShouldStartWith("Spectre.Cli.Tests.Data.Converters.CatAgilityConverter");
        }

        [Fact]
        public void Should_Shadow_Options_Declared_In_Parent_Command_If_Settings_Are_Of_Same_Type()
        {
            // Given
            var configurator = new Configurator(null);
            configurator.AddBranch<MammalSettings>("mammal", mammal =>
            {
                mammal.AddCommand<HorseCommand>("horse");
            });

            // When
            var model = CommandModelBuilder.Build(configurator);

            // Then
            model.Commands[0].As(mammal =>
            {
                mammal.GetOption(option => option.LongName == "name").As(o1 =>
                {
                    o1.ShouldNotBeNull();
                    o1.IsShadowed.ShouldBe(false);
                });

                mammal.Children[0].As(horse =>
                {
                    horse.GetOption(option => option.LongName == "name").As(o2 =>
                    {
                        o2.ShouldNotBeNull();
                        o2.IsShadowed.ShouldBe(true);
                    });
                });
            });
        }

        [Fact]
        public void Should_Make_Shadowed_Options_Non_Required()
        {
            // Given
            var configurator = new Configurator(null);
            configurator.AddBranch<MammalSettings>("mammal", mammal =>
            {
                mammal.AddCommand<HorseCommand>("horse");
            });

            // When
            var model = CommandModelBuilder.Build(configurator);

            // Then
            model.Commands[0].As(mammal =>
            {
                mammal.Children[0].As(horse =>
                {
                    horse.GetOption(option => option.LongName == "name").As(o2 =>
                    {
                        o2.ShouldNotBeNull();
                        o2.Required.ShouldBe(false);
                    });
                });
            });
        }

        [Fact]
        public void Should_Set_Default_Value_For_Options()
        {
            // Given
            var configurator = new Configurator(null);
            configurator.AddCommand<CatCommand>("cat");

            // When
            var model = CommandModelBuilder.Build(configurator);

            // Then
            model.Commands[0]
                .GetOption(option => option.LongName == "agility")
                .DefaultValue.Value.ShouldBe(10);
        }

        /// <summary>
        /// https://github.com/spectresystems/spectre.cli/wiki/Test-cases#test-case-1
        /// </summary>
        [Theory]
        [EmbeddedResourceData("Spectre.Cli.Tests/Data/Resources/Models/case1.xml")]
        public void Should_Generate_Correct_Model_For_Case_1(string expected)
        {
            // Given, When
            var result = CommandModelSerializer.Serialize(config =>
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

            // Then
            result.ShouldBe(expected);
        }

        /// <summary>
        /// https://github.com/spectresystems/spectre.cli/wiki/Test-cases#test-case-2
        /// </summary>
        [Theory]
        [EmbeddedResourceData("Spectre.Cli.Tests/Data/Resources/Models/case2.xml")]
        public void Should_Generate_Correct_Model_For_Case_2(string expected)
        {
            // Given, When
            var result = CommandModelSerializer.Serialize(config =>
            {
                config.AddCommand<DogCommand>("dog");
            });

            // Then
            result.ShouldBe(expected);
        }

        /// <summary>
        /// https://github.com/spectresystems/spectre.cli/wiki/Test-cases#test-case-3
        /// </summary>
        [Theory]
        [EmbeddedResourceData("Spectre.Cli.Tests/Data/Resources/Models/case3.xml")]
        public void Should_Generate_Correct_Model_For_Case_3(string expected)
        {
            // Given, When
            var result = CommandModelSerializer.Serialize(config =>
            {
                config.AddBranch<AnimalSettings>("animal", animal =>
                {
                    animal.AddCommand<DogCommand>("dog");
                    animal.AddCommand<HorseCommand>("horse");
                });
            });

            // Then
            result.ShouldBe(expected);
        }

        /// <summary>
        /// https://github.com/spectresystems/spectre.cli/wiki/Test-cases#test-case-4
        /// </summary>
        [Theory]
        [EmbeddedResourceData("Spectre.Cli.Tests/Data/Resources/Models/case4.xml")]
        public void Should_Generate_Correct_Model_For_Case_4(string expected)
        {
            // Given, When
            var result = CommandModelSerializer.Serialize(config =>
            {
                config.AddBranch<AnimalSettings>("animal", animal =>
                {
                    animal.AddCommand<DogCommand>("dog");
                });
            });

            // Then
            result.ShouldBe(expected);
        }

        [Fact]
        public void Should_Throw_If_Branch_Has_No_Children()
        {
            // Given
            var configurator = new Configurator(null);
            configurator.AddBranch<AnimalSettings>("animal", animal =>
            {
            });

            // When
            var model = Record.Exception(() => CommandModelBuilder.Build(configurator));

            // Then
            model.ShouldBeOfType<ConfigurationException>().And(exception =>
            {
                exception.Message.ShouldBe("The branch 'animal' does not define any commands.");
            });
        }
    }
}
