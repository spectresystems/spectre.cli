using System;
using Spectre.CommandLine.Internal.Rendering;

namespace Spectre.CommandLine.Internal
{
    internal sealed class ConfigurationException : CommandAppException
    {
        public override bool AlwaysPropagateWhenDebugging => true;

        public ConfigurationException(string message, IRenderable pretty = null)
            : base(message, pretty)
        {
        }

        public ConfigurationException(string message, Exception ex, IRenderable pretty = null)
            : base(message, ex, pretty)
        {
        }
    }
}