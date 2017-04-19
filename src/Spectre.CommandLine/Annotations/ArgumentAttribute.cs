using System;

// ReSharper disable once CheckNamespace
namespace Spectre.CommandLine
{
    public sealed class ArgumentAttribute : Attribute
    {
        public string Name { get; }

        public int Order { get; set; }

        public ArgumentAttribute(string name)
        {
            Name = name;
        }
    }
}