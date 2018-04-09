namespace Spectre.Cli.Internal.Rendering.Elements
{
    internal sealed class TabElement : IRenderable
    {
        private readonly RepeatingElement _element;

        public int Length => _element.Length;

        public TabElement(int count = 1)
        {
            _element = new RepeatingElement(count * 4, new TextElement(" "));
        }

        public void Render(IRenderer renderer)
        {
            _element.Render(renderer);
        }
    }
}