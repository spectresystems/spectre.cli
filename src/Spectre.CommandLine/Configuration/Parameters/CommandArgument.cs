// Licensed to Spectre Systems AB under one or more agreements.
// Spectre Systems AB licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Spectre.CommandLine.Configuration.Parameters
{
    internal sealed class CommandArgument : CommandParameter
    {
        public int Position { get; set; }
        public string Name { get; }

        public CommandArgument(ParameterInfo info, int position, string name, bool required)
            : base(info, required)
        {
            Position = position;
            Name = name;
        }
    }
}
