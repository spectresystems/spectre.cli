using System;
using System.IO;
using System.Reflection;
using Microsoft.Extensions.CommandLineUtils;
using Spectre.CommandLine.Internal;

namespace Spectre.CommandLine
{
    public abstract class CommandAppBase<TSettings> : IDisposable
        where TSettings : CommandAppSettings, new()
    {
        private readonly Mapper _mapper;

        internal CommandLineApplication App { get; }
        protected TSettings Settings { get; }
        protected IConsoleStreams Streams { get; set; }
        protected IResolver Resolver { get; }

        protected CommandAppBase(TSettings settings)
        {
            Settings = settings;

            Resolver = settings.Resolver ?? new DefaultResolver();
            Streams = settings.Streams ?? new ConsoleStreams();
            _mapper = new Mapper();

            App = new CommandLineApplication();
            App.HelpOption("-h | --help");
            App.Name = Path.GetFileNameWithoutExtension(Assembly.GetEntryAssembly().Location);
            App.FullName = App.Name;
            App.Out = Streams.Out;
            App.Error = Streams.Error;

            App.OnExecute(() =>
            {
                App.ShowHelp();
                return 0;
            });
        }

        public void Dispose()
        {
            Streams?.Dispose();
        }

        protected virtual void Initialize()
        {
        }

        protected void RegisterCommand(Type type)
        {
            var registrar = new CommandRegistrar(App, _mapper, Streams, Resolver);
            registrar.Register(type);
        }

        public int Run(string[] args)
        {
            try
            {
                Initialize();

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
            var output = Streams.Out.ToString().Trim();
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
            Streams.Error.WriteLine(ex.Message);
            Streams.Error.WriteLine();

            // Output the error stream.
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(Streams.Error.ToString().Trim());
            Console.ResetColor();
            Console.WriteLine();
        }
    }
}