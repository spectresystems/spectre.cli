using System;
using System.Collections.Generic;
using System.Text;

namespace Spectre.Cli.Internal
{
    internal static class Constants
    {
        public const string DefaultCommandName = "__default_command";
        public const string True = "true";
        public const string False = "false";

        public static string[] AcceptedBooleanValues { get; } = new string[]
        {
            True,
            False,
        };
    }
}
