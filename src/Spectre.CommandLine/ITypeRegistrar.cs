using System;

namespace Spectre.CommandLine
{
    public interface ITypeRegistrar
    {
        void Register(Type service, Type implementation);
        void RegisterInstance(Type service, object implementation);
        ITypeResolver Build();
    }
}
