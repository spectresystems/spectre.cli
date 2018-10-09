using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml;
using Spectre.Cli.Internal.Configuration;
using Spectre.Cli.Internal.Modelling;

namespace Spectre.Cli.Tests.Unit.Internal.Modelling
{
    internal static class CommandModelSerializer
    {
        public static string Serialize(Action<Configurator> func)
        {
            var configurator = new Configurator(null);
            func(configurator);

            var model = CommandModelBuilder.Build(configurator);

            var settings = new XmlWriterSettings
            {
                Indent = true,
                IndentChars = "  ",
                NewLineChars = "\n",
                OmitXmlDeclaration = false
            };

            using (var buffer = new StringWriter())
            using (var xmlWriter = XmlWriter.Create(buffer, settings))
            {
                Serialize(model).WriteTo(xmlWriter);
                xmlWriter.Flush();
                return buffer.GetStringBuilder().ToString().NormalizeLineEndings();
            }
        }

        private static XmlDocument Serialize(ICommandContainer model)
        {
            var document = new XmlDocument();
            var root = document.CreateElement("model");
            foreach (var command in model.Commands)
            {
                root.AppendChild(document.CreateComment(command.Name.ToUpperInvariant()));
                root.AppendChild(CreateCommandNode(document, command));
            }
            document.AppendChild(root);
            return document;
        }

        private static XmlNode CreateCommandNode(XmlDocument doc, CommandInfo command)
        {
            var node = doc.CreateElement("command");

            // Attributes
            node.SetNullableAttribute("name", command.Name);
            node.SetBooleanAttribute("isbranch", command.IsBranch);
            if (command.CommandType != null)
            {
                node.SetNullableAttribute("type", command.CommandType?.FullName);
            }
            node.SetNullableAttribute("settings", command.SettingsType?.FullName);

            // Parameters
            if (command.Parameters.Count > 0)
            {
                var parameterRootNode = doc.CreateElement("parameters");
                foreach (var parameter in CreateParameterNodes(doc, command))
                {
                    parameterRootNode.AppendChild(parameter);
                }
                node.AppendChild(parameterRootNode);
            }

            // Commands
            foreach (var childCommand in command.Children)
            {
                node.AppendChild(doc.CreateComment(childCommand.Name.ToUpperInvariant()));
                node.AppendChild(CreateCommandNode(doc, childCommand));
            }

            return node;
        }

        private static IEnumerable<XmlNode> CreateParameterNodes(XmlDocument document, CommandInfo command)
        {
            // Arguments
            foreach (var argument in command.Parameters.OfType<CommandArgument>().OrderBy(x => x.Position))
            {
                var node = document.CreateElement("argument");
                node.SetNullableAttribute("name", argument.Value);
                node.SetAttribute("position", argument.Position.ToString(CultureInfo.InvariantCulture));
                node.SetBooleanAttribute("required", argument.Required);
                node.SetEnumAttribute("kind", argument.ParameterKind);
                node.SetNullableAttribute("type", argument.ParameterType?.FullName);

                if (!string.IsNullOrWhiteSpace(argument.Description))
                {
                    var descriptionNode = document.CreateElement("description");
                    descriptionNode.InnerText = argument.Description;
                    node.AppendChild(descriptionNode);
                }

                if (argument.Validators.Count > 0)
                {
                    var validatorRootNode = document.CreateElement("validators");
                    foreach (var validator in argument.Validators.OrderBy(x => x.GetType().FullName))
                    {
                        var validatorNode = document.CreateElement("validator");
                        validatorNode.SetNullableAttribute("type", validator.GetType().FullName);
                        validatorNode.SetNullableAttribute("message", validator.ErrorMessage);
                        validatorRootNode.AppendChild(validatorNode);
                    }
                    node.AppendChild(validatorRootNode);
                }

                yield return node;
            }

            // Options
            foreach (var option in command.Parameters.OfType<CommandOption>()
                .OrderBy(x => string.Join(",", x.LongNames))
                .ThenBy(x => string.Join(",", x.ShortNames)))
            {
                var node = document.CreateElement("option");

                if (option.IsShadowed)
                {
                    node.SetBooleanAttribute("shadowed", true);
                }

                node.SetNullableAttribute("short", option.ShortNames);
                node.SetNullableAttribute("long", option.LongNames);
                node.SetNullableAttribute("value", option.ValueName);
                node.SetBooleanAttribute("required", option.Required);
                node.SetEnumAttribute("kind", option.ParameterKind);
                node.SetNullableAttribute("type", option.ParameterType?.FullName);

                if (!string.IsNullOrWhiteSpace(option.Description))
                {
                    var descriptionNode = document.CreateElement("description");
                    descriptionNode.InnerText = option.Description;
                    node.AppendChild(descriptionNode);
                }

                yield return node;
            }
        }
    }
}
