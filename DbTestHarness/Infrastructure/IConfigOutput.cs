using DbTestHarness.Models;

namespace DbTestHarness.Infrastructure;

public interface IConfigOutput
{
  string ServerType { get; }
  void Write(IEnumerable<ServerGroup> groups);
}