using System;
using System.ComponentModel;
using System.Reflection;
using System.Xml;

// ReSharper disable once CheckNamespace
namespace Spectre.CommandLine.Tests
{
    internal static class XmlElementExtensions
    {
        public static void SetNullableAttribute(this XmlElement element, string name, string value)
        {
            element.SetAttribute(name, value ?? "NULL");
        }

        public static void SetBooleanAttribute(this XmlElement element, string name, bool value)
        {
            element.SetAttribute(name, value ? "true" : "false");
        }

        public static void SetEnumAttribute(this XmlElement element, string name, Enum value)
        {
            var field = value.GetType().GetField(value.ToString());
            var attribute = field.GetCustomAttribute<DescriptionAttribute>(false);
            if (attribute == null)
            {
                throw new InvalidOperationException("Enum is missing description.");
            }
            element.SetAttribute(name, attribute.Description);
        }
    }
}
