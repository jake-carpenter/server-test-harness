using System.ComponentModel;
using Spectre.Console.Cli;

namespace Poke.Commands;

/// <summary>
/// Settings for adding a SQL Server entry via the 'new sqlserver' command.
/// </summary>
public sealed class NewSqlServerSettings : BaseSettings
{
    /// <summary>
    /// The group name for the SQL Server.
    /// </summary>
    [CommandOption("-g|--group")]
    [Description("Group name for the SQL Server")]
    public string? Group { get; init; }

    /// <summary>
    /// The data source (host name or IP address) of the SQL Server.
    /// </summary>
    [CommandOption("-d|--data-source")]
    [Description("Data source (host name or IP address) of the SQL Server")]
    public string? DataSource { get; init; }

    /// <summary>
    /// The instance name of the SQL Server.
    /// </summary>
    [CommandOption("-i|--instance")]
    [Description("Instance name of the SQL Server")]
    public string? Instance { get; init; }

    /// <summary>
    /// The connection string for the SQL Server.
    /// Takes precedence over data source if both are provided.
    /// </summary>
    [CommandOption("-s|--connection-string")]
    [Description("Connection string for the SQL Server (takes precedence over --data-source)")]
    public string? ConnectionString { get; init; }
}
