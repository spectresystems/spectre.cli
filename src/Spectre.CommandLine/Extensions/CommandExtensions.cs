// Licensed to Spectre Systems AB under one or more agreements.
// Spectre Systems AB licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;
using Spectre.CommandLine.Configuration;

// ReSharper disable once CheckNamespace
namespace Spectre.CommandLine
{
    internal static class CommandExtensions
    {
        public static CommandInfo FindCommand(this ICommandContainer container, string commandName)
        {
            return container.Commands.FirstOrDefault(c => c.Name.Equals(commandName, StringComparison.OrdinalIgnoreCase));
        }
    }
}
