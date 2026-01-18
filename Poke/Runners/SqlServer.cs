using System.Text.Json.Serialization;
using Poke.Models;

namespace Poke.Runners;

/// <summary>
/// SQL Server configuration entry.
/// </summary>
public record SqlServer : Server
{
    /// <summary>
    /// The server type discriminator.
    /// </summary>
    [JsonIgnore]
    public override string Type => "SqlServer";
}
