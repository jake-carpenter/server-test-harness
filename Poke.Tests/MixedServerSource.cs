using Poke.Models;
using Poke.Runners;

namespace Poke.Tests;

public record MixedServerSource
{
    public required IReadOnlyCollection<Server> Servers { get; init; }
}

public static partial class DataSources
{
    public static Func<MixedServerSource> MixedServers()
    {
        return () =>
            new MixedServerSource
            {
                Servers =
                [
                    new HttpServer
                    {
                        Id = Guid.NewGuid(),
                        GroupName = "Test Group 1",
                        Instance = "Test 1 Instance",
                        Uri = new Uri("https://example1.com"),
                    },
                    new HttpServer
                    {
                        Id = Guid.NewGuid(),
                        GroupName = "Test Group 2",
                        Instance = "Test 2 Instance",
                        Uri = new Uri("https://example2.com"),
                    },
                    new SqlServer
                    {
                        Id = Guid.NewGuid(),
                        GroupName = "Test Group 3",
                        Instance = "Test 3 Instance",
                        ConnectionString =
                            "Data Source=test3.com;Initial Catalog=test3;Integrated Security=True;",
                    },
                    new SqlServer
                    {
                        Id = Guid.NewGuid(),
                        GroupName = "Test Group 4",
                        Instance = "Test 4 Instance",
                        ConnectionString =
                            "Data Source=test4.com;Initial Catalog=test4;Integrated Security=True;",
                    },
                ],
            };
    }
}
