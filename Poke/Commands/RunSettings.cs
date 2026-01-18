using System.ComponentModel;
using Spectre.Console.Cli;

namespace Poke.Commands;

/// <summary>
/// Settings for running SQL Server checks.
/// </summary>
public sealed class RunSettings : BaseSettings
{
    /// <summary>
    /// Show detailed exception information.
    /// </summary>
    [CommandOption("-b|--debug")]
    [Description("Show detailed exception information")]
    public bool Debug { get; init; }

    /// <summary>
    /// Dry run the command without attempting any connections.
    /// </summary>
    [CommandOption("-d|--dry-run")]
    [Description("Dry run the command without attempting any connections")]
    public bool DryRun { get; init; }
}
