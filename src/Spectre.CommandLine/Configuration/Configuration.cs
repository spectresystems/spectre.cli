// Licensed to Spectre Systems AB under one or more agreements.
// Spectre Systems AB licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using Spectre.CommandLine.Annotations;

namespace Spectre.CommandLine.Configuration
{
    internal sealed class Configuration : IConfiguration
    {
        public string ApplicationName { get; set; }
        public OptionAttribute Help { get; set; }

        public ICollection<CommandInfo> Commands { get; }

        public Configuration()
        {
            Commands = new List<CommandInfo>();
            Help = new OptionAttribute("-h|--help");
        }
    }
}
