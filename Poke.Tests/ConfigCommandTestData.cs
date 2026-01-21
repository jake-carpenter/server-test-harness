using Poke.Models;

namespace Poke.Tests;

public record ConfigCommandTestData
{
    public List<Server> Servers { get; init; } = [];
}
