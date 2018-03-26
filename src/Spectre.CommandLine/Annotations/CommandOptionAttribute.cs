using System;
using Spectre.CommandLine.Internal.Parsing;

// ReSharper disable once CheckNamespace
namespace Spectre.CommandLine
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class CommandOptionAttribute : Attribute
    {
        public string LongName { get; }
        public string ShortName { get; }
        public string ValueName { get; }

        public CommandOptionAttribute(string template)
        {
            if (template == null)
            {
                throw new ArgumentNullException(nameof(template));
            }

            // Parse the option template.
            var result = TemplateParser.ParseOptionTemplate(template);

            // Assign the result.
            LongName = result.LongName;
            ShortName = result.ShortName;
            ValueName = result.Value;
        }
    }
}
