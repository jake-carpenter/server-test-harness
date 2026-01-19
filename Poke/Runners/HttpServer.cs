using System.Text.Json.Serialization;
using Poke.Models;

namespace Poke.Runners;

/// <summary>
/// HTTP Server configuration entry.
/// </summary>
public record HttpServer : Server
{
    /// <summary>
    /// The URI for the HTTP request.
    /// </summary>
    public required Uri Uri { get; init; }

    /// <summary>
    /// Whether to skip SSL certificate validation.
    /// </summary>
    public bool Insecure { get; init; }

    /// <summary>
    /// The server type discriminator.
    /// </summary>
    [JsonIgnore]
    public override string Type => "Http";
}
