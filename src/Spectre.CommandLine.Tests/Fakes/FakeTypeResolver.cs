using System;
using System.Collections.Generic;

namespace Spectre.CommandLine.Tests.Fakes
{
    public sealed class FakeTypeResolver : ITypeResolver
    {
        private readonly IDictionary<Type, object> _lookup;

        public FakeTypeResolver()
        {
            _lookup = new Dictionary<Type, object>();
        }

        public void Register<T>(T instance)
        {
            _lookup[typeof(T)] = instance;
        }

        public object Activate(Type type)
        {
            if (_lookup.TryGetValue(type, out var value))
            {
                return value;
            }
            return Activator.CreateInstance(type);
        }
    }
}
