using System.ComponentModel;
using Spectre.Console.Cli;

namespace Poke.Commands;

/// <summary>
/// Settings for adding an HTTP Server entry via the 'new http' command.
/// </summary>
public sealed class NewHttpSettings : BaseSettings
{
    /// <summary>
    /// The group name for the HTTP Server.
    /// </summary>
    [CommandOption("-g|--group")]
    [Description("Group name for the HTTP Server")]
    public string? Group { get; init; }

    /// <summary>
    /// The instance name of the HTTP Server.
    /// </summary>
    [CommandOption("-i|--instance")]
    [Description("Instance name of the HTTP Server")]
    public string? Instance { get; init; }

    /// <summary>
    /// The URI for the HTTP request.
    /// </summary>
    [CommandOption("-u|--uri")]
    [Description("URI for the HTTP request")]
    public string? Uri { get; init; }

    /// <summary>
    /// Whether to skip SSL certificate validation.
    /// </summary>
    [CommandOption("-x|--insecure")]
    [Description("Skip SSL certificate validation")]
    public bool Insecure { get; init; }
}
