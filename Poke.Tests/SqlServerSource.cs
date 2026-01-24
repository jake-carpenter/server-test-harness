using Poke.Runners;

namespace Poke.Tests;

public record SqlServerSource
{
    public required IReadOnlyCollection<SqlServer> Servers { get; init; }
}

public static partial class DataSources
{
    public static Func<SqlServerSource> SqlServers()
    {
        return () =>
            new SqlServerSource
            {
                Servers =
                [
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
