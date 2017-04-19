using System;

namespace Spectre.CommandLine.Internal
{
    internal sealed class DefaultResolver : IResolver
    {
        public object Resolve(Type type)
        {
            return Activator.CreateInstance(type);
        }
    }
}