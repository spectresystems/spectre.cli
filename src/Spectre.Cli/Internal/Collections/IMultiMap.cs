namespace Spectre.Cli.Internal
{
    internal interface IMultiMap
    {
        void Add((object? Key, object? Value) pair);
    }
}
