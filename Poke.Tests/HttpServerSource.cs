using Poke.Runners;

namespace Poke.Tests;

public record HttpServerSource
{
    public required IReadOnlyCollection<HttpServer> Servers { get; init; }
}

public static partial class DataSources
{
    public static Func<HttpServerSource> HttpServers()
    {
        return () =>
            new HttpServerSource
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
                    new HttpServer
                    {
                        Id = Guid.NewGuid(),
                        GroupName = "Test Group 1",
                        Instance = "Test 3 Instance",
                        Uri = new Uri("https://example3.com"),
                    },
                ],
            };
    }
}
