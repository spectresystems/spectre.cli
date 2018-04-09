using System;
using System.Diagnostics;
using Shouldly;

// ReSharper disable once CheckNamespace
namespace Spectre.Cli.Tests
{
    internal static class ShouldlyExtensions
    {
        [DebuggerStepThrough]
        public static T And<T>(this T obj, Action<T> action)
        {
            action(obj);
            return obj;
        }

        [DebuggerStepThrough]
        public static void As<T>(this T obj, Action<T> action)
        {
            action(obj);
        }

        [DebuggerStepThrough]
        public static void As<T>(this object obj, Action<T> action)
        {
            action((T)obj);
        }

        [DebuggerStepThrough]
        public static void ShouldBe<T>(this Type obj)
        {
            obj.ShouldBe(typeof(T));
        }
    }
}
