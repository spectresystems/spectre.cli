using System;

namespace Spectre.CommandLine.Annotations
{
    /// <summary>
    /// Represents an option or argument that is required.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class RequiredAttribute : Attribute
    {
    }
}
