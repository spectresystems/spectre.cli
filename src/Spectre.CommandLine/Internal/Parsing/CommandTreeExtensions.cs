using System.Linq;
using Spectre.CommandLine.Internal.Modelling;

namespace Spectre.CommandLine.Internal.Parsing
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

        public static bool HasArguments(this CommandTree tree)
        {
            return tree.Command.Parameters.OfType<CommandArgument>().Any();
        }

        public static CommandArgument FindArgument(this CommandTree tree, int position)
        {
            return tree.Command.Parameters
                .OfType<CommandArgument>()
                .FirstOrDefault(c => c.Position == position);
        }

        public static CommandOption FindOption(this CommandTree tree, string name, bool longOption)
        {
            return tree.Command.Parameters
                .OfType<CommandOption>()
                .FirstOrDefault(o => longOption ? o.LongName == name : o.ShortName == name);
        }

        public static bool IsMappedWithParent(this CommandTree tree, string name, bool longOption)
        {
            var node = tree.Parent;
            while (node != null)
            {
                var option = node.Command?.Parameters.OfType<CommandOption>()
                    .FirstOrDefault(o => longOption ? o.LongName == name : o.ShortName == name);

                if (option != null)
                {
                    if (node.Mapped.Any(p => p.Item1 == option))
                    {
                        return true;
                    }
                    return false;
                }

                node = node.Parent;
            }
            return false;
        }
    }
}
