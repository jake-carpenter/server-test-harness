using System.Text.Json.Serialization;
using Poke.Runners;

namespace Poke.Models;

/// <summary>
/// Base model for a server connection entry.
/// </summary>
[JsonPolymorphic]
[JsonDerivedType(typeof(SqlServer), typeDiscriminator: nameof(ServerType.SqlServer))]
[JsonDerivedType(typeof(HttpServer), typeDiscriminator: nameof(ServerType.Http))]
public abstract record Server
{
    /// <summary>
    /// The (generated) unique identifier for the server.
    /// </summary>
    public required Guid Id { get; init; }

    /// <summary>
    /// The group name the server belongs to.
    /// </summary>
    public required string GroupName { get; init; }

    /// <summary>
    /// The instance name of the server.
    /// </summary>
    public required string Instance { get; init; }

    /// <summary>
    /// The server type discriminator.
    /// </summary>
    public abstract ServerType Type { get; }
}
