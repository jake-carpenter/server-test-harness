using System.ComponentModel;
using Poke.Infrastructure;
using Poke.Models;
using Spectre.Console.Cli;

namespace Poke.Commands;

public class BaseSettings : CommandSettings
{
    private UserConfig? _userConfig;
    private readonly UserConfigManager _configManager = new();

    [CommandOption("-c|--config")]
    [Description("Configuration file to use")]
    public string? ConfigFile { get; init; }

    public UserConfig GetConfig()
    {
        _userConfig ??= _configManager.ReadConfigAsync(ConfigFile).GetAwaiter().GetResult();

        return _userConfig;
    }
}