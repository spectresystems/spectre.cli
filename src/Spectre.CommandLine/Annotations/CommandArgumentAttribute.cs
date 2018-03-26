using System;
using Spectre.CommandLine.Internal.Parsing;

// ReSharper disable once CheckNamespace
namespace Spectre.CommandLine
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class CommandArgumentAttribute : Attribute
    {
        public int Position { get; }
        public string Value { get; }
        public bool IsRequired { get; }

        public CommandArgumentAttribute(int position, string template)
        {
            if (template == null)
            {
                throw new ArgumentNullException(nameof(template));
            }

            // Parse the option template.
            var result = TemplateParser.ParseArgumentTemplate(template);

            // Assign the result.
            Position = position;
            Value = result.Value;
            IsRequired = result.Required;
        }
    }
}
