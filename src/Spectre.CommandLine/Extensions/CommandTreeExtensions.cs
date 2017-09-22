// Licensed to Spectre Systems AB under one or more agreements.
// Spectre Systems AB licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Linq;
using Spectre.CommandLine.Configuration.Parameters;
using Spectre.CommandLine.Parsing;

// ReSharper disable once CheckNamespace
namespace Spectre.CommandLine
{
    internal static class CommandTreeExtensions
    {
        public static CommandTree GetRootCommand(this CommandTree node)
        {
            while (node.Parent != null)
            {
                node = node.Parent;
            }
            return node;
        }

        public static CommandTree GetLeafCommand(this CommandTree node)
        {
            while (node.Next != null)
            {
                node = node.Next;
            }
            return node;
        }

        public static CommandOption FindOption(this CommandTree node, string name, bool isLongOption)
        {
            while (node != null)
            {
                var option = node.Command.Parameters
                    .OfType<CommandOption>()
                    .FirstOrDefault(o => isLongOption ? o.LongName == name : o.ShortName == name);

                if (option != null)
                {
                    return option;
                }

                node = node.Parent;
            }
            return null;
        }

        public static bool HasArguments(this CommandTree node)
        {
            return node.Command.Parameters.OfType<CommandArgument>().Any(x => !x.Parameter.IsInherited);
        }

        public static CommandArgument FindArgument(this CommandTree node, int argumentPosition)
        {
            return node.Command.Parameters
                .OfType<CommandArgument>()
                .SingleOrDefault(c => c.Position == argumentPosition && !c.Parameter.IsInherited);
        }
    }
}
