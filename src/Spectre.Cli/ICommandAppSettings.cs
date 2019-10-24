namespace Spectre.Cli
{
    /// <summary>
    /// Represents a command line application settings.
    /// </summary>
    public interface ICommandAppSettings
    {
        /// <summary>
        /// Gets or sets the application name.
        /// </summary>
        string? ApplicationName { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="IConsoleWriter"/>.
        /// </summary>
        IConsoleWriter? Console { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not parsing is strict.
        /// </summary>
        bool StrictParsing { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not exceptions should be propagated.
        /// </summary>
        bool PropagateExceptions { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not examples should be validated.
        /// </summary>
        bool ValidateExamples { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not XML documentation can be exported.
        /// This value can be overridden by setting the environment variable <c>SPECTRE_CLI_XMLDOC</c>.
        /// </summary>
        bool XmlDocEnabled { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not debug mode can be used.
        /// This value can be overridden by setting the environment variable <c>SPECTRE_CLI_DEBUG</c>.
        /// </summary>
        bool DebugEnabled { get; set; }
    }
}