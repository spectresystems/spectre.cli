using System.Reflection;
using Microsoft.Extensions.CommandLineUtils;

namespace Spectre.CommandLine.Internal.Mappings
{
    internal sealed class ArgumentMapping : Mapping
    {
        private readonly CommandArgument _argument;

        public override string Name => _argument.Name;

        public override bool HasValue => !string.IsNullOrWhiteSpace(_argument.Value);

        public override string Value => _argument.Value;

        public ArgumentMapping(PropertyInfo property, MappingType type, CommandArgument argument)
            : base(property, type)
        {
            _argument = argument;
        }
    }
}