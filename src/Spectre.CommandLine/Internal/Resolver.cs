using System;
using System.Collections.Generic;
using System.Text;

namespace Spectre.CommandLine.Internal
{
    internal class ResolverAdapter : IResolver
    {
        private readonly IResolver _resolver;

        public ResolverAdapter(IResolver resolver)
        {
            _resolver = resolver;
        }

        public object Resolve(Type type)
        {
            return _resolver?.Resolve(type) ?? Activator.CreateInstance(type);
        }
    }
}
