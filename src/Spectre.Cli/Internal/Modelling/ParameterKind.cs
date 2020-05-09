using System.ComponentModel;

namespace Spectre.Cli.Internal.Modelling
{
    internal enum ParameterKind
    {
        [Description("flag")]
        Flag = 0,
        [Description("flagvalue")]
        FlagWithValue = 1,
        [Description("scalar")]
        Scalar = 2,
        [Description("vector")]
        Vector = 3,
    }
}