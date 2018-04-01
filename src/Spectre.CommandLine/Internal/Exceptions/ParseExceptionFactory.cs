using System;
using System.Collections.Generic;
using Spectre.CommandLine.Internal.Parsing.Tokenization;
using Spectre.CommandLine.Internal.Rendering;

namespace Spectre.CommandLine.Internal.Exceptions
{
    internal static class ParseExceptionFactory
    {
        internal static ParseException Create(IReadOnlyList<string> arguments, CommandTreeToken token, string message, string details)
        {
            return new ParseException(message, CreatePrettyMessage(arguments, token, message, details));
        }

        private static IRenderable CreatePrettyMessage(IReadOnlyList<string> arguments, CommandTreeToken token, string message, string details)
        {
            var composer = new RenderableComposer();

            var position = token?.Position ?? 0;
            var value = token?.Representation ?? string.Join(" ", arguments);

            // Header
            composer.LineBreak();
            composer.Color(ConsoleColor.Red, error => error.Text("Error:"));
            composer.Space().Text(message);
            composer.LineBreak();

            // Template
            composer.LineBreak();
            composer.Spaces(7).Text(string.Join(" ", arguments));

            // Error
            composer.LineBreak();
            composer.Spaces(7).Spaces(position);
            composer.Color(ConsoleColor.Red, error =>
            {
                error.Repeat('^', value.Length);
                error.Text($" {details.TrimEnd('.')}");
                error.LineBreak();
            });

            composer.LineBreak();

            return composer;
        }
    }
}
