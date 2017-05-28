using System.Collections.Generic;

// ReSharper disable once CheckNamespace
namespace Spectre.CommandLine.Internal
{
    internal sealed class MappingCollection : List<IMapping>
    {
        public MappingCollection Parent { get; }

        public MappingCollection(MappingCollection parent)
        {
            Parent = parent;
        }

        public MappingCollection(MappingCollection parent, IEnumerable<IMapping> mappings)
            : base(mappings)
        {
            Parent = parent;
        }

        public Stack<MappingCollection> GetStack()
        {
            var result = new Stack<MappingCollection>();
            result.Push(this);

            var current = this;
            while (current.Parent != null)
            {
                result.Push(current.Parent);
                current = current.Parent;
            }

            return result;
        }
    }
}
