using System;
using System.Collections.Generic;

namespace Spectre.CommandLine.Tests.Testing
{
    public sealed class TestResolver : IResolver
    {
        private readonly IDictionary<Type, object> _lookup;

        public TestResolver()
        {
            _lookup = new Dictionary<Type, object>();
        }

        public void Register<T>(T instance)
        {
            _lookup[typeof(T)] = instance;
        }

        public object Resolve(Type type)
        {
            if (_lookup.ContainsKey(type))
            {
                return _lookup[type];
            }
            return null;
        }
    }
}
