using DbTestHarness.Models;

namespace DbTestHarness.Runners;

public record SqlServer : Server
{
  public override string Type => "SqlServer";
}