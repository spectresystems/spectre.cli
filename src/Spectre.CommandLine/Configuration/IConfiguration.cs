// Licensed to Spectre Systems AB under one or more agreements.
// Spectre Systems AB licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Spectre.CommandLine.Annotations;

namespace Spectre.CommandLine.Configuration
{
    internal interface IConfiguration : ICommandContainer
    {
        string ApplicationName { get; }
        OptionAttribute Help { get; }
    }
}