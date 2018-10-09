using System;
using System.Collections.Generic;
using System.Linq;
using Spectre.Cli.Internal.Configuration;

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
        public IReadOnlyList<string> LongNames { get; }

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
        public CommandOptionAttribute(string template)
        {
            if (template == null)
            {
                throw new ArgumentNullException(nameof(template));
            }

            // Parse the option template.
            var result = TemplateParser.ParseOptionTemplate(template);

            // Assign the result.
            LongNames = result.LongNames;
            ShortName = result.ShortName;
            ValueName = result.Value;
        }

        internal bool IsMatch(string name)
        {
            return
                ShortName?.Equals(name, StringComparison.Ordinal) == true ||
                LongNames.Contains(name, StringComparer.Ordinal);
        }
    }
}
