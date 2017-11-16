using System.ComponentModel;

namespace Spectre.CommandLine.Internal.Modelling
{
    internal enum ParameterKind
    {
        [Description("flag")]
        Flag = 0,
        [Description("single")]
        Single = 1
    }
}