using System;

namespace Spectre.CommandLine
{
    public interface ITypeResolver
    {
        object Resolve(Type type);
    }
}
