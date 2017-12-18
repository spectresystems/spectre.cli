using System;
using System.Collections.Generic;

namespace Spectre.CommandLine.Tests.Fakes
{
    public sealed class FakeTypeRegistrar : ITypeRegistrar
    {
        public Dictionary<Type, List<Type>> Registrations { get; }

        public FakeTypeRegistrar()
        {
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
    }
}
