namespace Spectre.Cli.Internal.Rendering.Elements
{
    internal sealed class RepeatingElement : IRenderable
    {
        private readonly int _repetitions;
        private readonly IRenderable _element;

        public int Length => _element.Length * _repetitions;

        public RepeatingElement(int repetitions, IRenderable element)
        {
            _repetitions = repetitions;
            _element = element;
        }

        public void Render(IRenderer renderer)
        {
            for (var index = 0; index < _repetitions; index++)
            {
                _element.Render(renderer);
            }
        }
    }
}
