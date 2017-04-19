using System;

namespace Spectre.CommandLine
{
    public static class CommandAppExtensions
    {
        public static void SetExecutableName(this CommandApp app, string name)
        {
            app.App.Name = name;
        }

        public static void SetTitle(this CommandApp app, string title)
        {
            app.App.FullName = title;
        }

        public static void SetVersion(this CommandApp app, string version, string longVersion = null)
        {
            if (version == null)
            {
                throw new ArgumentNullException(nameof(version));
            }
            app.App.VersionOption("-v | --version", version, longVersion ?? version);
        }

        public static void SetHelpText(this CommandApp app, string text)
        {
            app.App.ExtendedHelpText = text;
        }
    }
}
