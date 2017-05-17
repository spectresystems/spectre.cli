using System;

namespace Spectre.CommandLine
{
    public static class CommandAppExtensions
    {
        public static void SetExecutableName<TSettings>(this CommandAppBase<TSettings> appBase, string name)
            where TSettings : CommandAppSettings, new()
        {
            appBase.App.Name = name;
        }

        public static void SetTitle<TSettings>(this CommandAppBase<TSettings> appBase, string title)
            where TSettings : CommandAppSettings, new()
        {
            appBase.App.FullName = title;
        }

        public static void SetVersion<TSettings>(this CommandAppBase<TSettings> appBase, string version, string longVersion = null)
            where TSettings : CommandAppSettings, new()
        {
            if (version == null)
            {
                throw new ArgumentNullException(nameof(version));
            }
            appBase.App.VersionOption("-v | --version", version, longVersion ?? version);
        }

        public static void SetHelpText<TSettings>(this CommandAppBase<TSettings> appBase, string text)
            where TSettings : CommandAppSettings, new()
        {
            appBase.App.ExtendedHelpText = text;
        }
    }
}
