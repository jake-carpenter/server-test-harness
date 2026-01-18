using System.ComponentModel;
using Spectre.Console.Cli;

namespace Poke.Commands;

/// <summary>
/// Common settings shared by commands that accept a configuration file.
/// </summary>
public class BaseSettings : CommandSettings
{
    /// <summary>
    /// The configuration file path to use.
    /// </summary>
    [CommandOption("-c|--config")]
    [Description("Configuration file to use")]
    public string? ConfigFile { get; init; }
}
