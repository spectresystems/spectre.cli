using System;
using Spectre.CommandLine.Internal.Rendering;
using Spectre.CommandLine.Internal.Templating;

namespace Spectre.CommandLine.Internal
{
    internal sealed class TemplateException : CommandAppException
    {
        public string Template { get; }
        public string Summary { get; }
        public int Position { get; }

        public override bool AlwaysPropagateWhenDebugging => true;

        internal TemplateException(string message, string template, int position, string summary, IRenderable pretty)
            : base(message, pretty)
        {
            Template = template;
            Position = position;
            Summary = summary;
        }

        internal static class Factory
        {
            internal static TemplateException Create(string template, TemplateToken token, string message, string summary = null)
            {
                return Create(template, token?.Position ?? 0, token?.Representation ?? template, message, summary);
            }

            internal static TemplateException Create(string template, int position, string value, string message, string summary = null)
            {
                summary = summary ?? message;
                var pretty = CreateRenderableMessage(template, position, value, message, summary);
                return new TemplateException(message, template, position, summary, pretty);
            }

            private static IRenderable CreateRenderableMessage(string template, int position, string value, string message, string details)
            {
                var composer = new RenderableComposer();

                // Header
                composer.LineBreak();
                composer.Color(ConsoleColor.Red, error => error.Text("Error:"));
                composer.Space().Text("An error occured when parsing template.");
                composer.LineBreak();
                composer.Spaces(7).Color(ConsoleColor.Yellow, error => error.Text(message));
                composer.LineBreak();

                if (string.IsNullOrWhiteSpace(template))
                {
                    // Error
                    composer.LineBreak();
                    composer.Color(ConsoleColor.Red, error => error.Text(message));
                    composer.LineBreak();
                }
                else
                {
                    // Template
                    composer.LineBreak();
                    composer.Spaces(7).Text(template);

                    // Error
                    composer.LineBreak();
                    composer.Spaces(7).Spaces(position);
                    composer.Color(ConsoleColor.Red, error =>
                    {
                        error.Repeat('^', value.Length);
                        error.Text($" {details}");
                        error.LineBreak();
                    });
                }

                composer.LineBreak();

                return composer;
            }
        }
    }
}
