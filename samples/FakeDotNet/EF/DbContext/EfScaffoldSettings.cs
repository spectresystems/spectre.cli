using System.ComponentModel;
using Spectre.CommandLine;

namespace FakeDotNet.EF.DbContext
{
    public sealed class EfScaffoldSettings : EfCommandSettings
    {
        [CommandArgument(0, "<CONNECTION>")]
        [Description("The connection string to the database.")]
        public string Connection { get; set; }

        [CommandArgument(1, "<PROVIDER>")]
        [Description("The provider to use. (E.g. Microsoft.EntityFrameworkCore.SqlServer)")]
        public string Provider { get; set; }

        [CommandOption("-f|--force")]
        [Description("Overwrite existing files.")]
        public bool Force { get; set; }

        [CommandOption("-o|--output-dir [PATH]")]
        [Description("The directory to put files in. Paths are relative to the project directory.")]
        public string OutputDir { get; set; }

        [CommandOption("--schema [SCHEMA_NAME]")]
        [Description("The schemas of tables to generate entity types for.")]
        public string Schema { get; set; }

        [CommandOption("-t|--table [TABLE_NAME]")]
        [Description("The tables to generate entity types for.")]
        public string Table { get; set; }

        [CommandOption("--use-database-names")]
        [Description("Use table and column names directly from the database.")]
        public bool UseDatabaseNames { get; set; }
    }
}