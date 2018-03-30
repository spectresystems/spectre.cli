using System;
using Spectre.CommandLine.Internal.Rendering;
using Spectre.CommandLine.Internal.Templating;

namespace Spectre.CommandLine.Internal.Exceptions
{
    internal static class TemplateExceptionFactory
    {
        internal static TemplateException Create(string template, TemplateToken token, string message, string details)
        {
            return new TemplateException(message, template, CreatePrettyMessage(template, token, message, details));
        }

        private static IRenderable CreatePrettyMessage(string template, TemplateToken token, string message, string details)
        {
            var composer = new RenderableComposer();

            var position = token?.Position ?? 0;
            var value = token?.Representation ?? template;

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
                    error.Text($" {details.TrimEnd('.')}");
                    error.LineBreak();
                });
            }

            composer.LineBreak();

            return composer;
        }
    }
}
