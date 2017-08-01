using System.Collections.Generic;
using Shouldly;
using Spectre.CommandLine.Tests.Data;
using Xunit;

namespace Spectre.CommandLine.Tests.Unit
{
    public sealed class CommandAppTests
    {
        public sealed class TheRunMethod
        {
            public static IEnumerable<object[]> Arguments()
            {
                yield return new object[] {new[] {"foo", "--foo", "Hello", "bar", "--bar", "2"}};
                yield return new object[] {new[] {"foo", "--foo", "Hello", "bar", "-b", "2"}};
                yield return new object[] {new[] {"foo", "-f", "Hello", "bar", "-b", "2"}};
                yield return new object[] {new[] {"foo", "-f", "Hello", "bar", "--bar", "2"}};
            }

            [Theory]
            [MemberData(nameof(Arguments))]
            public void Should_Execute_Command_With_Specified_Options(string[] args)
            {
                // Given
                var fixture = new Fixture();

                // When
                var result = fixture.Run(args);

                // Then
                result.ShouldBe(0);
                fixture.CallRecorder.WasCalled("FooBar Foo=Hello Bar=2 Qux=0").ShouldBeTrue();
            }

            [Fact]
            public void Should_Use_Default_Value_For_Options_If_Specified()
            {
                // Given
                var fixture = new Fixture();

                // When
                var result = fixture.Run(new[] {"foo", "--foo", "1", "bar"});

                // Then
                result.ShouldBe(0);
                fixture.CallRecorder.WasCalled("FooBar Foo=1 Bar=3 Qux=0").ShouldBeTrue();
            }

            [Fact]
            public void Should_Use_Type_Converter_For_Options_If_Specified()
            {
                // Given
                var fixture = new Fixture();

                // When
                var result = fixture.Run(new[] {"foo", "--foo", "1", "bar", "--bar", "1", "--qux", "HelloWorld"});

                // Then
                result.ShouldBe(0);
                fixture.CallRecorder.WasCalled("FooBar Foo=1 Bar=1 Qux=10").ShouldBeTrue();
            }
        }

        public sealed class Fixture
        {
            public CallRecorder CallRecorder { get; set; }
            public TestResolver Resolver { get; set; }

            public Fixture()
            {
                CallRecorder = new CallRecorder();

                Resolver = new TestResolver();
                Resolver.Register(new BarCommand(CallRecorder));
                Resolver.Register(new BarSettings());
            }

            public int Run(string[] args)
            {
                var app = new CommandApp(Resolver);
                app.Configure(config =>
                {
                    config.AddProxy<FooSettings>("foo", foo =>
                    {
                        foo.AddCommand<BarCommand>("bar");
                    });
                });
                return app.Run(args);
            }
        }
    }
}
