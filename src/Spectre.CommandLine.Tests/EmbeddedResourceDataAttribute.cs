using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Xunit.Sdk;

namespace Spectre.CommandLine.Tests
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    public sealed class EmbeddedResourceDataAttribute : DataAttribute
    {
        private readonly string[] _args;

        public EmbeddedResourceDataAttribute(params string[] args)
        {
            _args = args;
        }

        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            var result = new object[_args.Length];
            for (var index = 0; index < _args.Length; index++)
            {
                result[index] = ReadManifestData(_args[index]);
            }
            return new[] { result };
        }

        public static string ReadManifestData(string resourceName)
        {
            var assembly = typeof(EmbeddedResourceDataAttribute).GetTypeInfo().Assembly;
            resourceName = resourceName.Replace("/", ".", StringComparison.Ordinal);
            using (var stream = assembly.GetManifestResourceStream(resourceName))
            {
                if (stream == null)
                {
                    throw new InvalidOperationException("Could not load manifest resource stream.");
                }
                using (var reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd().NormalizeLineEndings();
                }
            }
        }
    }
}