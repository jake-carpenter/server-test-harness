using System.ComponentModel;
using Spectre.Console.Cli;

namespace Poke.Commands;

/// <summary>
/// Settings for adding a SQL Server entry.
/// </summary>
public sealed class AddSettings : BaseSettings
{
    /// <summary>
    /// The group name for the SQL Server.
    /// </summary>
    [CommandOption("-g|--group")]
    [Description("Group name for the SQL Server")]
    public string? Group { get; init; }

    /// <summary>
    /// The host name or IP address of the SQL Server.
    /// </summary>
    [CommandOption("-h|--host")]
    [Description("Host name or IP address of the SQL Server")]
    public string? Host { get; init; }

    /// <summary>
    /// The instance name of the SQL Server.
    /// </summary>
    [CommandOption("-i|--instance")]
    [Description("Instance name of the SQL Server")]
    public string? Instance { get; init; }
}
