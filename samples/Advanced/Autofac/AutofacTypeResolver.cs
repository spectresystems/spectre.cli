using System;
using Autofac;
using Spectre.Cli;

namespace Sample.Autofac
{
    public sealed class AutofacTypeResolver : ITypeResolver
    {
        private readonly ILifetimeScope _scope;

        public AutofacTypeResolver(ILifetimeScope scope)
        {
            _scope = scope;
        }

        public object Resolve(Type type)
        {
            return _scope.Resolve(type);
        }
    }
}