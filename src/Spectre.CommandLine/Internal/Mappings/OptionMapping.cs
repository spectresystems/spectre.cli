using System.Reflection;
using Microsoft.Extensions.CommandLineUtils;

namespace Spectre.CommandLine.Internal.Mappings
{
    internal sealed class OptionMapping : Mapping
    {
        private readonly CommandOption _option;

        public override string Name => _option.LongName;

        public override bool HasValue => _option.HasValue();

        public override string Value => _option.Value();

        public OptionMapping(PropertyInfo property, MappingType type, CommandOption option)
            : base(property, type)
        {
            _option = option;
        }
    }
}