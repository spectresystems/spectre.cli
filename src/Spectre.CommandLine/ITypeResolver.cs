using System;

namespace Spectre.CommandLine
{
    public interface ITypeResolver
    {
        object Activate(Type type);
    }
}
