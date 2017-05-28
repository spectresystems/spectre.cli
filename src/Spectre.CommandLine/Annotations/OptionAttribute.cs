using System;

// ReSharper disable once CheckNamespace
namespace Spectre.CommandLine
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class OptionAttribute : Attribute
    {
        public string Template { get; }

        public OptionAttribute(string template)
        {
            Template = template;
        }
    }
}