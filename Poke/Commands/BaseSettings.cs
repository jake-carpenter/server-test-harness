using System.ComponentModel;
using Spectre.Console.Cli;

namespace Poke.Commands;

public class BaseSettings : CommandSettings
{
    [CommandOption("-c|--config")]
    [Description("Configuration file to use")]
    public string? ConfigFile { get; init; }
}
