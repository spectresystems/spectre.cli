using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using Shouldly;
using Spectre.Cli.Internal.Configuration;
using Spectre.Cli.Internal.Modelling;
using Spectre.Cli.Internal.Parsing;
using Spectre.Cli.Tests.Data;
using Spectre.Cli.Tests.Data.Settings;
using Xunit;

namespace Spectre.Cli.Tests.Unit.Internal.Parsing
{
    public sealed class CommandTreeParserTests
    {
        [Fact]
        public void Should_Capture_Remaining_Arguments()
        {
            // Given, When
            var (_, remaining) = new Fixture().Parse(new[] { "dog", "--", "--foo", "-bar", "\"baz\"", "qux" }, config =>
            {
                config.AddCommand<DogCommand>("dog");
            });

            // Then
            remaining.Count.ShouldBe(4);
            remaining[0].ShouldBe("--foo");
            remaining[1].ShouldBe("-bar");
            remaining[2].ShouldBe("\"baz\"");
            remaining[3].ShouldBe("qux");
        }

        /// <summary>
        /// https://github.com/spectresystems/spectre.cli/wiki/Test-cases#test-case-1
        /// </summary>
        [Theory]
        [EmbeddedResourceData("Spectre.Cli.Tests/Data/Resources/Parsing/case1.xml")]
        public void Should_Parse_Correct_Tree_For_Case_1(string expected)
        {
            // Given, When
            var result = new Fixture().Serialize(
                new[] { "animal", "--alive", "mammal", "--name", "Rufus", "dog", "12", "--good-boy" },
                config =>
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
        [EmbeddedResourceData("Spectre.Cli.Tests/Data/Resources/Parsing/case2.xml")]
        public void Should_Parse_Correct_Tree_For_Case_2(string expected)
        {
            // Given, When
            var result = new Fixture().Serialize(
                new[] { "dog", "12", "4", "--good-boy", "--name", "Rufus", "--alive" },
                config =>
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
        [EmbeddedResourceData("Spectre.Cli.Tests/Data/Resources/Parsing/case3.xml")]
        public void Should_Parse_Correct_Tree_For_Case_3(string expected)
        {
            // Given, When
            var result = new Fixture().Serialize(
                new[] { "animal", "dog", "12", "--good-boy", "--name", "Rufus" },
                config =>
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
        [EmbeddedResourceData("Spectre.Cli.Tests/Data/Resources/Parsing/case4.xml")]
        public void Should_Parse_Correct_Tree_For_Case_4(string expected)
        {
            // Given, When
            var result = new Fixture().Serialize(
                new[] { "animal", "4", "dog", "12", "--good-boy", "--name", "Rufus" },
                config =>
                {
                    config.AddBranch<AnimalSettings>("animal", animal =>
                    {
                        animal.AddCommand<DogCommand>("dog");
                    });
                });

            // Then
            result.ShouldBe(expected);
        }

        [Theory]
        [EmbeddedResourceData("Spectre.Cli.Tests/Data/Resources/Parsing/default1.xml")]
        [EmbeddedResourceData("Spectre.Cli.Tests/Data/Resources/Parsing/default2.xml", "--good-boy")]
        [EmbeddedResourceData("Spectre.Cli.Tests/Data/Resources/Parsing/default3.xml", "--help")]
        [EmbeddedResourceData("Spectre.Cli.Tests/Data/Resources/Parsing/default4.xml", "4", "12", "--good-boy")]
        public void Should_Use_Default_Command_If_No_Command_Was_Specified(string expected, params string[] args)
        {
            // Given, When
            var result = new Fixture()
                .WithDefaultCommand<DogCommand>()
                .Serialize(
                    args, config =>
                    {
                        config.AddCommand<CatCommand>("cat");
                    });

            // Then
            result.ShouldBe(expected);
        }

        [Fact]
        public void Should_Not_Use_Default_Command_If_Command_Was_Specified()
        {
            // Given, When
            var (tree, _) = new Fixture()
                .WithDefaultCommand<DogCommand>()
                .Parse(new[] { "cat" }, config =>
            {
                config.AddCommand<CatCommand>("cat");
            });

            // Then
            tree.Command.CommandType.ShouldBe<CatCommand>();
            tree.Command.SettingsType.ShouldBe<CatSettings>();
        }

        private sealed class Fixture
        {
            private Type _defaultCommand;

            public Fixture WithDefaultCommand<TCommand>()
            {
                _defaultCommand = typeof(TCommand);
                return this;
            }

            public (CommandTree, IReadOnlyList<string> remaining) Parse(IEnumerable<string> args, Action<Configurator> func)
            {
                var configurator = new Configurator(null, _defaultCommand);
                func(configurator);

                var model = CommandModelBuilder.Build(configurator);
                return new CommandTreeParser(model).Parse(args);
            }

            public string Serialize(IEnumerable<string> args, Action<Configurator> func)
            {
                var (tree, _) = Parse(args, func);

                var settings = new XmlWriterSettings
                {
                    Indent = true,
                    IndentChars = "  ",
                    NewLineChars = "\n",
                    OmitXmlDeclaration = false
                };

                using (var buffer = new StringWriter())
                using (var xmlWriter = XmlWriter.Create(buffer, settings))
                {
                    CommandTreeSerializer.Serialize(tree).WriteTo(xmlWriter);
                    xmlWriter.Flush();
                    return buffer.GetStringBuilder().ToString().NormalizeLineEndings();
                }
            }
        }
    }
}
