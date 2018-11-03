using System;
using System.Collections.Generic;
using System.Xml;
using Spectre.Cli.Internal.Modelling;
using Spectre.Cli.Internal.Parsing;

namespace Spectre.Cli.Tests.Unit.Internal.Parsing
{
    internal static class CommandTreeSerializer
    {
        public static XmlDocument Serialize(CommandTree tree)
        {
            var document = new XmlDocument();
            var root = document.CreateElement("model");
            root.AppendChild(document.CreateComment(tree.Command.Name.ToUpperInvariant()));
            root.AppendChild(Serialize(document, tree));
            document.AppendChild(root);
            return document;
        }

        private static XmlNode Serialize(XmlDocument doc, CommandTree tree)
        {
            var node = doc.CreateElement("command");

            // Attributes
            node.SetNullableAttribute("name", tree.Command.Name);
            node.SetBooleanAttribute("help", tree.ShowHelp);

            // Mapped
            if (tree.Mapped.Count > 0)
            {
                var parameterRootNode = doc.CreateElement("mapped");
                foreach (var parameter in CreateMappedParameterNodes(doc, tree.Mapped))
                {
                    parameterRootNode.AppendChild(parameter);
                }
                node.AppendChild(parameterRootNode);
            }

            // Unmapped
            if (tree.Unmapped.Count > 0)
            {
                var parameterRootNode = doc.CreateElement("unmapped");
                foreach (var parameter in CreateUnmappedParameterNodes(doc, tree.Unmapped))
                {
                    parameterRootNode.AppendChild(parameter);
                }
                node.AppendChild(parameterRootNode);
            }

            // Command
            if (tree.Next != null)
            {
                node.AppendChild(doc.CreateComment(tree.Next.Command.Name.ToUpperInvariant()));
                node.AppendChild(Serialize(doc, tree.Next));
            }

            return node;
        }

        private static IEnumerable<XmlNode> CreateMappedParameterNodes(XmlDocument document, List<MappedCommandParameter> parameters)
        {
            foreach (var parameter in parameters)
            {
                if (parameter.Parameter is CommandOption option)
                {
                    var node = document.CreateElement("option");
                    node.SetNullableAttribute("name", option.GetOptionName());
                    node.SetNullableAttribute("assigned", parameter.Value);
                    yield return node;
                }
                else if (parameter.Parameter is CommandArgument argument)
                {
                    var node = document.CreateElement("argument");
                    node.SetNullableAttribute("name", argument.Value);
                    node.SetNullableAttribute("assigned", parameter.Value);
                    yield return node;
                }
            }
        }

        private static IEnumerable<XmlNode> CreateUnmappedParameterNodes(XmlDocument document, List<CommandParameter> parameters)
        {
            foreach (var parameter in parameters)
            {
                if (parameter is CommandOption option)
                {
                    var node = document.CreateElement("option");
                    node.SetNullableAttribute("name", option.GetOptionName());
                    yield return node;
                }
                else if (parameter is CommandArgument argument)
                {
                    var node = document.CreateElement("argument");
                    node.SetNullableAttribute("name", argument.Value);
                    yield return node;
                }
            }
        }
    }
}
