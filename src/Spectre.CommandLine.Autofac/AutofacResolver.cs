using System;
using Autofac;

namespace Spectre.CommandLine.Autofac
{
    public sealed class AutofacResolver : IResolver
    {
        private readonly ILifetimeScope _scope;

        public AutofacResolver(ILifetimeScope scope)
        {
            _scope = scope;
        }

        public object Resolve(Type type)
        {
            return _scope.Resolve(type);
        }
    }
}
