using System.Text.Json.Serialization;
using DbTestHarness.Runners;

namespace DbTestHarness.Models;


[JsonPolymorphic]
[JsonDerivedType(typeof(SqlServer), typeDiscriminator: "SqlServer")]
public abstract record Server
{
    public required string GroupName { get; init; }
    public required string Instance { get; init; }
    public required string Host { get; init; }
    public abstract string Type { get; }
}
