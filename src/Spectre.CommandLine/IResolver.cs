using System;

namespace Spectre.CommandLine
{
    public interface IResolver
    {
        object Resolve(Type type);
    }
}
