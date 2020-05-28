using System;

namespace Spectre.Cli.Internal
{
    internal interface IPairDeconstructor
    {
        (object? Key, object? Value) Deconstruct(
            ITypeResolver resolver,
            Type keyType,
            Type valueType,
            string? value);
    }
}
