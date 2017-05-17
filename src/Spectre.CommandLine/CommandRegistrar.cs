using System;
using System.ComponentModel;
using System.Reflection;
using Microsoft.Extensions.CommandLineUtils;
using Spectre.CommandLine.Internal;

namespace Spectre.CommandLine
{
    internal sealed class CommandRegistrar : ICommandRegistrar
    {
        private readonly CommandLineApplication _app;
        private readonly Mapper _mapper;
        private readonly IConsoleStreams _streams;
        private readonly IResolver _resolver;

        public CommandRegistrar(CommandLineApplication app, Mapper mapper, IConsoleStreams streams, IResolver resolver)
        {
            _app = app;
            _mapper = mapper;
            _streams = streams;
            _resolver = resolver;
        }

        public void Register(Type type)
        {
            var commandType = typeof(ICommand);
            if (!commandType.GetTypeInfo().IsAssignableFrom(type))
            {
                throw new InvalidOperationException($"Could not register command of type '{type.FullName}' since it does not implement '{commandType.FullName}'.");
            }

            // Resolve the command.
            var command = _resolver.Resolve(type) as ICommand;
            if (command == null)
            {
                throw new InvalidOperationException("Could not resolve command.");
            }

            _app.Command(command.Name, c =>
            {
                // Is this a proxy?
                var isProxy = typeof(ProxyCommand).GetTypeInfo().IsAssignableFrom(command.GetType());

                // Register help option for command.
                c.HelpOption("-h | --help");
                c.Out = _streams.Out;
                c.Error = _streams.Error;

                // Command got a description?
                var description = command.GetType().GetTypeInfo().GetCustomAttribute<DescriptionAttribute>();
                if (description != null)
                {
                    c.FullName = description.Description;
                }

                // Allow this command to register additional commands.
                var registar = new CommandRegistrar(c, _mapper, _streams, _resolver);
                command.Configure(registar);

                if (isProxy)
                {
                    c.OnExecute(() =>
                    {
                        c.ShowHelp();
                        return 0;
                    });
                }
                else
                {
                    // Create mappings.
                    var mappings = MappingFactory.CreateMappings(c, command.SettingsType);

                    // Register execution callback.
                    c.OnExecute(() =>
                    {
                        var settings = _resolver.Resolve(command.SettingsType);
                        foreach (var mapping in mappings)
                        {
                            _mapper.Map(settings, mapping);
                        }
                        return command.Run(settings);
                    });
                }
            });
        }
    }
}
