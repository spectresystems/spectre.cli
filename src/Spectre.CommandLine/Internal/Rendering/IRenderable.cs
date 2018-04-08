namespace Spectre.CommandLine.Internal.Rendering
{
    internal interface IRenderable
    {
        int Length { get; }
        void Render(IRenderer renderer);
    }
}