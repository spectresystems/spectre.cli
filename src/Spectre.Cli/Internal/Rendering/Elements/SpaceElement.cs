namespace Spectre.Cli.Internal.Rendering
{
    internal sealed class SpaceElement : IRenderable
    {
        private readonly RepeatingElement _element;

        public int Length => _element.Length;

        public SpaceElement(int count = 1)
        {
            _element = new RepeatingElement(count, new TextElement(" "));
        }

        public void Render(IRenderer renderer)
        {
            _element.Render(renderer);
        }
    }
}