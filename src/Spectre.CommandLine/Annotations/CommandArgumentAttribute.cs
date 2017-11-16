using System;

// ReSharper disable once CheckNamespace
namespace Spectre.CommandLine
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class CommandArgumentAttribute : Attribute
    {
        public int Position { get; }
        public string Value { get; }
        public bool IsRequired { get; }

        public CommandArgumentAttribute(int position, string value)
        {
            Position = position;
            Value = value;

            if (IsRequiredOrOptionalArgument(Value))
            {
                IsRequired = IsRequiredArgument(Value);
                Value = TrimArgument(Value);
            }
        }

        private static bool IsRequiredOrOptionalArgument(string text)
        {
            if (text != null)
            {
                return (text.StartsWith("[", StringComparison.OrdinalIgnoreCase) && text.EndsWith("]", StringComparison.OrdinalIgnoreCase)) ||
                       (text.StartsWith("<", StringComparison.OrdinalIgnoreCase) && text.EndsWith(">", StringComparison.OrdinalIgnoreCase));
            }
            return false;
        }

        private static bool IsRequiredArgument(string text)
        {
            if (text != null)
            {
                return text.StartsWith("<", StringComparison.OrdinalIgnoreCase) && text.EndsWith(">", StringComparison.OrdinalIgnoreCase);
            }
            return false;
        }

        private static string TrimArgument(string text)
        {
            return text?.TrimStart('[', '<')?.TrimEnd(']', '>');
        }
    }
}
