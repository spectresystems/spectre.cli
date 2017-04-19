using System;
using System.Collections.Generic;
using System.Text;
using Spectre.CommandLine;

namespace Example.Commands.Nuget
{
    public sealed class NuGetCommand : ProxyCommand
    {
        public NuGetCommand()
            : base("nuget")
        {
        }

        public override void Configure(ICommandRegistrar registrar)
        {
            registrar.Register<NuGetPushCommand>();
            registrar.Register<NuGetDeleteCommand>();
        }
    }
}
