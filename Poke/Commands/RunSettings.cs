using System.ComponentModel;
using Spectre.Console.Cli;

namespace Poke.Commands;

public sealed class RunSettings : BaseSettings
{
    [CommandOption("-b|--debug")]
    [Description("Show detailed exception information")]
    public bool Debug { get; init; }

    [CommandOption("-d|--dry-run")]
    [Description("Dry run the command without attempting any connections")]
    public bool DryRun { get; init; }
}
