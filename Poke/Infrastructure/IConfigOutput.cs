using Poke.Models;

namespace Poke.Infrastructure;

public interface IConfigOutput
{
  string ServerType { get; }
  void Write(IEnumerable<ServerGroup> groups);
}