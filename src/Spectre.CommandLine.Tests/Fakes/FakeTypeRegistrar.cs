using System;
using System.Collections.Generic;

namespace Spectre.CommandLine.Tests.Fakes
{
    public sealed class FakeTypeRegistrar : ITypeRegistrar
    {
        private readonly ITypeResolver _resolver;
        public Dictionary<Type, List<Type>> Registrations { get; }
        public Dictionary<Type, List<object>> Instances { get; }

        public FakeTypeRegistrar(ITypeResolver resolver = null)
        {
            _resolver = resolver;
            Registrations = new Dictionary<Type, List<Type>>();
            Instances = new Dictionary<Type, List<object>>();
        }

        public void Register(Type service, Type implementation)
        {
            if (!Registrations.ContainsKey(service))
            {
                Registrations.Add(service, new List<Type> { implementation });
            }
            else
            {
                Registrations[service].Add(implementation);
            }
        }

        public void RegisterInstance(Type service, object implementation)
        {
            if (!Instances.ContainsKey(service))
            {
                Instances.Add(service, new List<object> { implementation });
            }
            else
            {
                Instances[service].Add(implementation);
            }
        }

        public ITypeResolver Build()
        {
            return _resolver;
        }
    }
}
