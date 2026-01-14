using Poke.Models;

namespace Poke.Runners;

public record SqlServer : Server
{
  public override string Type => "SqlServer";
}