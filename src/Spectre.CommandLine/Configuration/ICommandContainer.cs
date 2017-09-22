// Licensed to Spectre Systems AB under one or more agreements.
// Spectre Systems AB licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;

namespace Spectre.CommandLine.Configuration
{
    internal interface ICommandContainer
    {
        ICollection<CommandInfo> Commands { get; }
    }
}
