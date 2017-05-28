using System.ComponentModel;

// ReSharper disable once CheckNamespace
namespace Spectre.CommandLine.Internal
{
    internal static class Mapper
    {
        public static void Map(object settings, MappingCollection mappings)
        {
            var stack = mappings.GetStack();
            while (stack.Count != 0)
            {
                var current = stack.Pop();
                foreach (var mapping in current)
                {
                    Map(settings, mapping);
                }
            }
        }

        private static void Map(object settings, IMapping mapping)
        {
            if (!mapping.HasValue)
            {
                if (mapping.HasDefaultValue)
                {
                    mapping.Assign(settings, mapping.DefaultValue);
                }
                return;
            }

            // Flag mapping?
            if (mapping.Kind == MappingType.Flag)
            {
                // NOTE: The "on" value is a boolean convention in Microsoft.Extensions.CommandLineUtils.
                mapping.Assign(settings, mapping.Value == "on");
            }

            // Scalar mapping?
            if (mapping.Kind == MappingType.Scalar)
            {
                var converter = TypeDescriptor.GetConverter(mapping.Type);
                var value = converter.ConvertFromInvariantString(mapping.Value);
                mapping.Assign(settings, value);
            }
        }
    }
}
