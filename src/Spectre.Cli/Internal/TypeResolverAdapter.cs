using System;
using Spectre.Cli.Exceptions;

namespace Spectre.Cli.Internal
{
    internal sealed class TypeResolverAdapter : ITypeResolver
    {
        private readonly ITypeResolver? _resolver;

        public TypeResolverAdapter(ITypeResolver? resolver)
        {
            _resolver = resolver;
        }

        public object? Resolve(Type? type)
        {
            if (type == null)
            {
                throw new RuntimeException("Cannot resolve null type.");
            }

            try
            {
                if (_resolver != null)
                {
                    var obj = _resolver.Resolve(type);
                    if (obj == null)
                    {
                        throw RuntimeException.CouldNotResolveType(type);
                    }

                    return obj;
                }

                // Fall back to use the activator.
                return Activator.CreateInstance(type);
            }
            catch (CommandAppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw RuntimeException.CouldNotResolveType(type, ex);
            }
        }
    }
}
