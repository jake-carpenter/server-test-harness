using Spectre.Console.Cli;

namespace DbTestHarness.Models;

public sealed class Settings : CommandSettings
{
    [CommandOption("-c|--config")]
    public string ConfigFile { get; init; } = "servers.json";

    [CommandOption("-d|--dry-run")]
    public bool DryRun { get; init; }
}