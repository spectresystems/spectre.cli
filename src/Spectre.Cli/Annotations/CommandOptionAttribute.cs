using System;
using Spectre.Cli.Internal.Templating;

// ReSharper disable once CheckNamespace
namespace Spectre.Cli
{
    /// <summary>
    /// An attribute representing a command option.
    /// </summary>
    /// <seealso cref="Attribute" />
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class CommandOptionAttribute : Attribute
    {
        /// <summary>
        /// Gets the long name of the option.
        /// </summary>
        /// <value>The long name of the option.</value>
        public string LongName { get; }

        /// <summary>
        /// Gets the short name of the option.
        /// </summary>
        /// <value>The short name of the option.</value>
        public string ShortName { get; }

        /// <summary>
        /// Gets the value name of the option.
        /// </summary>
        /// <value>The value name of the option.</value>
        public string ValueName { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandOptionAttribute"/> class.
        /// </summary>
        /// <param name="template">The option template.</param>
        /// <exception cref="ArgumentNullException">template</exception>
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
