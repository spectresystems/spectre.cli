using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using Spectre.Cli.Exceptions;
using Spectre.Cli.Internal.Modelling;
using Spectre.Cli.Internal.Parsing;

namespace Spectre.Cli.Internal
{
    internal static class CommandBinder
    {
        [SuppressMessage("Style", "IDE0019:Use pattern matching", Justification = "It's OK")]
        public static void Bind(CommandTree? tree, ref CommandSettings settings, ITypeResolver resolver)
        {
            ValidateRequiredParameters(tree);

            TypeConverter? GetConverter(CommandSettings settings, CommandParameter parameter)
            {
                if (parameter.Converter == null)
                {
                    if (parameter.ParameterType.IsArray)
                    {
                        // Return a converter for each array item (not the whole array)
                        return TypeDescriptor.GetConverter(parameter.ParameterType.GetElementType());
                    }

                    if (parameter.IsFlagValue())
                    {
                        // Is the optional value instanciated?
                        var value = parameter.Property.GetValue(settings) as IFlagValue;
                        if (value == null)
                        {
                            // Try to assign it with a null value.
                            // This will create the optional value instance without a value.
                            parameter.Assign(settings, null);
                            value = parameter.Property.GetValue(settings) as IFlagValue;
                            if (value == null)
                            {
                                throw new InvalidOperationException("Could not intialize optional value.");
                            }
                        }

                        // Return a converter for the flag element type.
                        return TypeDescriptor.GetConverter(value.Type);
                    }

                    return TypeDescriptor.GetConverter(parameter.ParameterType);
                }

                var type = Type.GetType(parameter.Converter.ConverterTypeName);
                return resolver.Resolve(type) as TypeConverter;
            }

            while (tree != null)
            {
                // Process unmapped parameters.
                foreach (var parameter in tree.Unmapped)
                {
                    if (parameter.IsFlagValue())
                    {
                        // Set the flag value to an empty, not set instance.
                        var instance = Activator.CreateInstance(parameter.ParameterType);
                        parameter.Property.SetValue(settings, instance);
                    }
                    else
                    {
                        // Is this an option with a default value?
                        if (parameter is CommandOption option && option.DefaultValue != null)
                        {
                            parameter.Assign(settings, option.DefaultValue?.Value);
                            ValidateParameter(parameter, settings);
                        }
                    }
                }

                // Process mapped parameters.
                foreach (var mapped in tree.Mapped)
                {
                    var converter = GetConverter(settings, mapped.Parameter);
                    if (converter == null)
                    {
                        throw RuntimeException.NoConverterFound(mapped.Parameter);
                    }

                    if (mapped.Parameter.IsFlagValue() && mapped.Value == null)
                    {
                        if (mapped.Parameter is CommandOption option && option.DefaultValue != null)
                        {
                            // Set the default value.
                            mapped.Parameter.Assign(settings, option.DefaultValue?.Value);
                        }
                        else
                        {
                            // Set the flag but not the value.
                            mapped.Parameter.Assign(settings, null);
                        }
                    }
                    else
                    {
                        // Assign the value to the parameter.
                        mapped.Parameter.Assign(settings, converter.ConvertFromInvariantString(mapped.Value));
                    }

                    ValidateParameter(mapped.Parameter, settings);
                }

                tree = tree.Next;
            }

            // Validate the settings.
            var validationResult = settings.Validate();
            if (!validationResult.Successful)
            {
                throw RuntimeException.ValidationFailed(validationResult);
            }
        }

        private static void ValidateRequiredParameters(CommandTree? tree)
        {
            var node = tree?.GetRootCommand();
            while (node != null)
            {
                foreach (var parameter in node.Unmapped)
                {
                    if (parameter.Required)
                    {
                        switch (parameter)
                        {
                            case CommandArgument argument:
                                throw RuntimeException.MissingRequiredArgument(node, argument);
                        }
                    }
                }

                node = node.Next;
            }
        }

        private static void ValidateParameter(CommandParameter parameter, CommandSettings settings)
        {
            var assignedValue = parameter.Get(settings);
            foreach (var validator in parameter.Validators)
            {
                var validationResult = validator.Validate(parameter, assignedValue);
                if (!validationResult.Successful)
                {
                    // If there is a error message specified in the parameter validator attribute,
                    // then use that one, otherwise use the validation result.
                    var result = validator.ErrorMessage != null
                        ? ValidationResult.Error(validator.ErrorMessage)
                        : validationResult;

                    throw RuntimeException.ValidationFailed(result);
                }
            }
        }
    }
}
