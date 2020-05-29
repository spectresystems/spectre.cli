namespace Spectre.Cli.Internal.Collections
{
    internal interface IMultiMap
    {
        void Add((object? Key, object? Value) pair);
    }
}
