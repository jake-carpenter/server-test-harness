using System.ComponentModel;
using Spectre.Console.Cli;

namespace Poke.Commands;

public sealed class AddSettings : BaseSettings
{
    [CommandOption("-g|--group")]
    [Description("Group name for the SQL Server")]
    public string? Group { get; init; }

    [CommandOption("-h|--host")]
    [Description("Host name or IP address of the SQL Server")]
    public string? Host { get; init; }

    [CommandOption("-i|--instance")]
    [Description("Instance name of the SQL Server")]
    public string? Instance { get; init; }
}
