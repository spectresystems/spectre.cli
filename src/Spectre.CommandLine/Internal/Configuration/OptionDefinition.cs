using System;
using System.Reflection;

// ReSharper disable once CheckNamespace
namespace Spectre.CommandLine.Internal
{
    internal sealed class OptionDefinition
    {
        public Type Type { get; set; }

        public PropertyInfo Property { get; set; }

        public bool Inherited { get; set; }

        public string Template { get; set; }
    }
}