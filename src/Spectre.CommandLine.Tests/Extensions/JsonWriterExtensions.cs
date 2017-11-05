using System;
using System.ComponentModel;
using System.Reflection;
using Newtonsoft.Json;

// ReSharper disable once CheckNamespace
namespace Spectre.CommandLine.Tests
{
    internal static class JsonWriterExtensions
    {
        public static void WriteProperty(this JsonWriter writer, string name, string value)
        {
            writer.WritePropertyName(name);
            writer.WriteValue(value ?? "NULL");
        }

        public static void WriteProperty(this JsonWriter writer, string name, bool value)
        {
            writer.WriteProperty(name, value ? "true" : "false");
        }

        public static void WriteProperty(this JsonWriter writer, string name, Enum value)
        {
            var field = value.GetType().GetField(value.ToString());
            var attribute = field.GetCustomAttribute<DescriptionAttribute>(false);
            if (attribute == null)
            {
                throw new InvalidOperationException("Enum is missing description.");
            }
            writer.WriteProperty(name, attribute.Description);
        }
    }
}
