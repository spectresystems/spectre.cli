// Licensed to Spectre Systems AB under one or more agreements.
// Spectre Systems AB licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.ComponentModel;
using Spectre.CommandLine.Configuration;
using Spectre.CommandLine.Configuration.Parameters;
using Spectre.CommandLine.Parsing;

namespace Spectre.CommandLine
{
    internal sealed class CommandSettingsFactory
    {
        private readonly ITypeResolver _resolver;

        public CommandSettingsFactory(ITypeResolver resolver)
        {
            _resolver = resolver;
        }

        public object CreateSettings(CommandTree root, Type settingsType)
        {
            var settings = _resolver.Resolve(settingsType);

            TypeConverter GetConverter(CommandParameter parameter)
            {
                if (parameter.Parameter.Converter == null)
                {
                    return TypeDescriptor.GetConverter(parameter.Parameter.Type);
                }
                var type = Type.GetType(parameter.Parameter.Converter.ConverterTypeName);
                return _resolver.Resolve(type) as TypeConverter;
            }

            while (root != null)
            {
                // Process mapped parameters.
                foreach (var (parameter, value) in root.Mapped)
                {
                    if (!parameter.Parameter.IsInherited)
                    {
                        var converter = GetConverter(parameter);
                        parameter.Assign(settings, converter.ConvertFromInvariantString(value));
                    }
                }

                // Process unmapped parameters.
                foreach (var parameter in root.Unmapped)
                {
                    if (!parameter.Parameter.IsInherited)
                    {
                        // Is this an option with a default value?
                        if (parameter is CommandOption option && option.DefaultValue != null)
                        {
                            parameter.Assign(settings, option.DefaultValue.Value);
                        }
                    }
                }

                root = root.Next;
            }

            return settings;
        }
    }
}
