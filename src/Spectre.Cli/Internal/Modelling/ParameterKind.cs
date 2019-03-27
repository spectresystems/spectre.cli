using System.ComponentModel;

namespace Spectre.Cli.Internal.Modelling
{
    internal enum ParameterKind
    {
        [Description("flag")]
        Flag = 0,
        [Description("scalar")]
        Scalar = 1,
        [Description("vector")]
        Vector = 2
    }
}