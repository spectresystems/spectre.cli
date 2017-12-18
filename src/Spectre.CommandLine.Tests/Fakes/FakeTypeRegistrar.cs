using System;
using System.Collections.Generic;
using System.Linq;

namespace Spectre.CommandLine.Tests.Fakes
{
    public sealed class FakeTypeRegistrar : ITypeRegistrar
    {
        private readonly ITypeResolver _resolver;
        public Dictionary<Type, List<Type>> Registrations { get; }

        public FakeTypeRegistrar(ITypeResolver resolver = null)
        {
            _resolver = resolver;
            Registrations = new Dictionary<Type, List<Type>>();
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

        public ITypeResolver Build()
        {
            return _resolver;
        }
    }
}
