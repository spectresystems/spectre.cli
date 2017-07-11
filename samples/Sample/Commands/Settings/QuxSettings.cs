using System;
using System.ComponentModel;
using Spectre.CommandLine.Annotations;

namespace Sample.Commands.Settings
{
    public sealed class QuxSettings : BazSettings
    {
        [Option("-q|--qux <FLUX_CONDENSATOR>")]
        [Description("Sets the qux timestamp for the current baz.")]
        public DateTime Qux { get; set; }
    }
}