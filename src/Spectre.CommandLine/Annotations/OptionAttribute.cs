using System;

namespace Spectre.CommandLine.Annotations
{
    /// <summary>
    /// Represents an option.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class OptionAttribute : Attribute
    {
        /// <summary>
        /// Gets the long name.
        /// </summary>
        /// <value>The long name.</value>
        public string LongName { get; }

        /// <summary>
        /// Gets the short name.
        /// </summary>
        /// <value>The short name.</value>
        public string ShortName { get; }

        /// <summary>
        /// Gets the value name.
        /// </summary>
        /// <value>The value name.</value>
        public string ValueName { get; }

        /// <summary>
        /// Gets a value indicating whether this option is required.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this option is required; otherwise, <c>false</c>.
        /// </value>
        public bool IsRequired { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="OptionAttribute"/> class.
        /// </summary>
        /// <param name="template">The option template.</param>
        public OptionAttribute(string template)
        {
            if (template == null)
            {
                throw new ArgumentNullException(nameof(template));
            }

            var parts = template.Split(new[] { ' ', '|' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var part in parts)
            {
                if (part.StartsWith("--"))
                {
                    if (part.Length > 3)
                    {
                        LongName = part.Substring(2);
                        continue;
                    }
                    throw new CommandAppException("Invalid long option.");
                }
                if (part.StartsWith("-"))
                {
                    if (part.Length == 2)
                    {
                        ShortName = part.Substring(1);
                        continue;
                    }
                    throw new CommandAppException("Invalid short option.");
                }
                if (part.StartsWith("<") && part.EndsWith(">") ||
                    part.StartsWith("[") && part.EndsWith("]"))
                {
                    if (part.Length > 2)
                    {
                        ValueName = part.Substring(1, part.Length - 2);
                        IsRequired = part.StartsWith("[") && part.EndsWith("]");
                        continue;
                    }
                }

                throw new InvalidOperationException("Invalid template pattern.");
            }

            if (string.IsNullOrWhiteSpace(ShortName) && string.IsNullOrWhiteSpace(LongName))
            {
                throw new InvalidOperationException("Invalid template pattern.");
            }
        }
    }
}