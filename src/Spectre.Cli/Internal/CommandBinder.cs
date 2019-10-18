using System;
using System.ComponentModel;
using Spectre.Cli.Internal.Exceptions;
using Spectre.Cli.Internal.Modelling;
using Spectre.Cli.Internal.Parsing;

namespace Spectre.Cli.Internal
{
    internal static class CommandBinder
    {
        public static void Bind(CommandTree? tree, ref CommandSettings settings, ITypeResolver resolver)
        {
            ValidateRequiredParameters(tree);

            TypeConverter? GetConverter(CommandParameter parameter)
            {
                if (parameter.Converter == null)
                {
                    if (parameter.ParameterType.IsArray)
                    {
                        // Return a converter for each array item (not the whole array)
                        return TypeDescriptor.GetConverter(parameter.ParameterType.GetElementType());
                    }

                    return TypeDescriptor.GetConverter(parameter.ParameterType);
                }
                var type = Type.GetType(parameter.Converter.ConverterTypeName);
                return resolver.Resolve(type) as TypeConverter;
            }

            while (tree != null)
            {
                // Process mapped parameters.
                foreach (var mapped in tree.Mapped)
                {
                    var converter = GetConverter(mapped.Parameter);
                    if (converter == null)
                    {
                        throw RuntimeException.NoConverterFound(mapped.Parameter);
                    }

                    mapped.Parameter.Assign(settings, converter.ConvertFromInvariantString(mapped.Value));
                    ValidateParameter(mapped.Parameter, settings);
                }

                // Process unmapped parameters.
                foreach (var parameter in tree.Unmapped)
                {
                    // Is this an option with a default value?
                    if (parameter is CommandOption option && option.DefaultValue != null)
                    {
                        parameter.Assign(settings, option.DefaultValue?.Value);
                        ValidateParameter(parameter, settings);
                    }
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
                var validationResult = validator.Validate(assignedValue);
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
