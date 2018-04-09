namespace Spectre.Cli.Internal.Rendering
{
    internal interface IRenderable
    {
        int Length { get; }
        void Render(IRenderer renderer);
    }
}