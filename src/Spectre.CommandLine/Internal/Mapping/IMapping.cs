using System;
using System.ComponentModel;

// ReSharper disable once CheckNamespace
namespace Spectre.CommandLine.Internal
{
    internal interface IMapping
    {
        Type Type { get; }
        MappingType Kind { get; }
        bool Required { get; }

        bool HasValue { get; }
        string Value { get; }

        bool HasDefaultValue { get; }
        object DefaultValue { get; }

        TypeConverter Converter { get; }

        void Assign(object settings, object value);
    }
}
