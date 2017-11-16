using System;
using Shouldly;

// ReSharper disable once CheckNamespace
namespace Spectre.CommandLine.Tests
{
    internal static class ShouldlyExtensions
    {
        public static T And<T>(this T obj)
        {
            return obj;
        }

        public static T And<T>(this T obj, Action<T> action)
        {
            action(obj);
            return obj;
        }

        public static void As<T>(this T obj, Action<T> action)
        {
            action(obj);
        }

        public static void ShouldBe<T>(this Type obj)
        {
            obj.ShouldBe(typeof(T));
        }
    }
}
