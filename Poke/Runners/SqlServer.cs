using System.Text.Json.Serialization;
using Microsoft.Data.SqlClient;
using Poke.Models;

namespace Poke.Runners;

/// <summary>
/// SQL Server configuration entry.
/// </summary>
public record SqlServer : Server
{
    /// <summary>
    /// The connection string for the server.
    /// </summary>
    public required string ConnectionString { get; init; }

    /// <summary>
    /// The parsed connection string builder.
    /// </summary>
    [JsonIgnore]
    public SqlConnectionStringBuilder ConnectionStringBuilder => new(ConnectionString);

    /// <summary>
    /// The data source (host) extracted from the connection string.
    /// </summary>
    [JsonIgnore]
    public string DataSource => ConnectionStringBuilder.DataSource;

    /// <summary>
    /// The server type discriminator.
    /// </summary>
    [JsonIgnore]
    public override string Type => "SqlServer";
}
