// Licensed to Spectre Systems AB under one or more agreements.
// Spectre Systems AB licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace Spectre.CommandLine.Utilities
{
    internal sealed class TypeResolverAdapter : ITypeResolver
    {
        private readonly ITypeResolver _resolver;

        public TypeResolverAdapter(ITypeResolver resolver)
        {
            _resolver = resolver;
        }

        public object Resolve(Type type)
        {
            try
            {
                if (_resolver != null)
                {
                    var obj = _resolver.Resolve(type);
                    if (obj == null)
                    {
                        throw new CommandAppException($"Could not resolve type '{type.FullName}'.");
                    }

                    return obj;
                }

                // Fall back to use the activator.
                return Activator.CreateInstance(type);
            }
            catch (Exception ex)
            {
                throw new CommandAppException($"Could not resolve type '{type.FullName}'.", ex);
            }
        }
    }
}
