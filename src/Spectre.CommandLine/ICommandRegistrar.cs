using System;

namespace Spectre.CommandLine
{
    public interface ICommandRegistrar
    {
        void Register(Type type);
    }
}
