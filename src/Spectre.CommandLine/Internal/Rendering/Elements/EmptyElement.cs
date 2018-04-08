namespace Spectre.CommandLine.Internal.Rendering.Elements
{
    internal sealed class EmptyElement : IRenderable
    {
        public int Length => 0;

        public void Render(IRenderer renderer)
        {
        }
    }
}