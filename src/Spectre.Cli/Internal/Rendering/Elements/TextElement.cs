namespace Spectre.Cli.Internal
{
    internal sealed class TextElement : IRenderable
    {
        private readonly string _text;

        public int Length => _text.Length;

        public TextElement(string text)
        {
            _text = text;
        }

        public void Render(IRenderer renderer)
        {
            renderer.Append(_text);
        }
    }
}