using System.Reflection;

namespace Spectre.CommandLine.Internal
{
    internal abstract class Mapping
    {
        public PropertyInfo Property { get; }

        public MappingType Type { get; }

        public bool Required { get; set; }

        public abstract string Name { get; }

        public abstract bool HasValue { get; }

        public abstract string Value { get; }

        protected Mapping(PropertyInfo property, MappingType type)
        {
            Property = property;
            Type = type;
        }
    }
}
