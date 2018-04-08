namespace Spectre.CommandLine.Internal.Rendering.Elements
{
    internal sealed class TabElement : IRenderable
    {
        public int Length => 4;

        public void Render(IRenderer renderer)
        {
            renderer.Append("    ");
        }
    }
}