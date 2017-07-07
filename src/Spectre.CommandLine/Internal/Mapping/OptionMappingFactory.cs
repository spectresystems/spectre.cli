using Microsoft.Extensions.CommandLineUtils;

// ReSharper disable once CheckNamespace
namespace Spectre.CommandLine.Internal
{
    internal sealed class OptionMappingFactory : MappingFactory<OptionDefinition>
    {
        public override IMapping CreateMapping(CommandLineApplication app, OptionDefinition item)
        {
            // Inherited?
            if (item.Inherited)
            {
                return null;
            }

            // Create the option.
            var optionType = item.MappingType == MappingType.Flag ? CommandOptionType.NoValue : CommandOptionType.SingleValue;
            var option = app.Option(item.Template, item.Description, optionType, item.Inherited);

            // Create the mapping.
            return new OptionMapping(item, option);
        }
    }
}
