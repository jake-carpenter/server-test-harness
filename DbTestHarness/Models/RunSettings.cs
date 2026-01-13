using System.ComponentModel;
using Spectre.Console.Cli;

namespace DbTestHarness.Models;

public sealed class RunSettings : CommandSettings
{
    [CommandOption("-b|--debug")]
    [Description("Show detailed exception information")]
    public bool Debug { get; init; }

    [CommandOption("-c|--config")]
    [Description("Configuration file to use")]
    public string ConfigFile { get; init; } = "servers.json";

    [CommandOption("-d|--dry-run")]
    [Description("Dry run the command without attempting any connections")]
    public bool DryRun { get; init; }
}