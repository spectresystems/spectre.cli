using System;

namespace Spectre.CommandLine
{
    public interface ITypeRegistrar
    {
        void Register(Type service, Type implementation);
        ITypeResolver Build();
    }
}
