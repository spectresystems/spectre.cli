// Licensed to Spectre Systems AB under one or more agreements.
// Spectre Systems AB licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Spectre.CommandLine.Parsing
{
    internal class CommandTreeParserContext
    {
        public int CurrentArgumentPosition { get; private set; }

        public void ResetArgumentPosition()
        {
            CurrentArgumentPosition = 0;
        }

        public void IncreaseArgumentPosition()
        {
            CurrentArgumentPosition += 1;
        }
    }
}