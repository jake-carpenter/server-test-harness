using System.ComponentModel;
using Spectre.Console.Cli;

namespace DbTestHarness.Models;

public sealed class Settings : CommandSettings
{
    [CommandOption("-b|--debug")]
    [Description("Show detailed exception information")]
    public bool Debug { get; init; }

    [CommandOption("-c|--config")]
    public string ConfigFile { get; init; } = "servers.json";

    [CommandOption("-d|--dry-run")]
    public bool DryRun { get; init; }
}