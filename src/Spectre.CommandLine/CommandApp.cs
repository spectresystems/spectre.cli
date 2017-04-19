using System;
using System.IO;
using System.Reflection;
using Microsoft.Extensions.CommandLineUtils;
using Spectre.CommandLine.Internal;

namespace Spectre.CommandLine
{
    public sealed class CommandApp : IDisposable
    {
        private readonly IResolver _resolver;
        private readonly Mapper _mapper;
        private readonly ConsoleStreams _streams;

        internal CommandLineApplication App { get; }

        public CommandApp()
            : this(null)
        {
        }

        public CommandApp(IResolver resolver)
        {
            _resolver = resolver ?? new DefaultResolver();
            _mapper = new Mapper();
            _streams = new ConsoleStreams();

            App = new CommandLineApplication();
            App.HelpOption("-h | --help");
            App.Name = Path.GetFileNameWithoutExtension(Assembly.GetEntryAssembly().Location);
            App.FullName = App.Name;
            App.Out = _streams.Out;
            App.Error = _streams.Error;

            App.OnExecute(() =>
            {
                App.ShowHelp();
                return 0;
            });
        }

        public void Dispose()
        {
            _streams?.Dispose();
        }

        public void Register<TCommand>()
            where TCommand : ICommand
        {
            var registrar = new CommandRegistrar(App, _mapper, _streams, _resolver);
            registrar.Register<TCommand>();
        }

        public int Run(string[] args)
        {
            try
            {
                var result = App.Execute(args);
                PrintOutput();
                return result;
            }
            catch (CommandParsingException ex)
            {
                PrintError(ex);
                return 1;
            }
            catch (CommandLineException ex)
            {
                PrintError(ex);
                return 1;
            }
        }

        private void PrintOutput()
        {
            var output = _streams.Out.ToString().Trim();
            if (!string.IsNullOrWhiteSpace(output))
            {
                Console.WriteLine();
                Console.WriteLine(output);
                Console.WriteLine();
            }
        }

        private void PrintError(Exception ex)
        {
            // Write the exception to the error stream.
            _streams.Error.WriteLine(ex.Message);
            _streams.Error.WriteLine();

            // Output the error stream.
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(_streams.Error.ToString().Trim());
            Console.ResetColor();
            Console.WriteLine();
        }
    }
}