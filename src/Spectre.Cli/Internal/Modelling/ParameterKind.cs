using System.ComponentModel;

namespace Spectre.Cli.Internal.Modelling
{
    internal enum ParameterKind
    {
        [Description("flag")]
        Flag = 0,
        [Description("single")]
        Single = 1,
        [Description("multiple")]
        Multiple = 2
    }
}