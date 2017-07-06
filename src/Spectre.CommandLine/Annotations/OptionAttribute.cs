using System;

// ReSharper disable once CheckNamespace
namespace Spectre.CommandLine
{
    /// <summary>
    /// Represents an option.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class OptionAttribute : Attribute
    {
        /// <summary>
        /// The option template.
        /// </summary>
        public string Template { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="OptionAttribute"/> class.
        /// </summary>
        /// <param name="template">The option template.</param>
        public OptionAttribute(string template)
        {
            Template = template;
        }
    }
}