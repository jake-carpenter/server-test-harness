namespace DbTestHarness.Models;

public record SqlServer : Server
{
  public override string Type => "SqlServer";
}