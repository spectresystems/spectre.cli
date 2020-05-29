namespace Spectre.Cli.Internal
{
    internal interface IRenderable
    {
        int Length { get; }
        void Render(IRenderer renderer);
    }
}