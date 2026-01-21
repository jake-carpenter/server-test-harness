using Poke.Runners;
using Poke.Tests.Commands;

namespace Poke.Tests;

public static class DataSources
{
    public static Func<ConfigCommandTestData> ConfigCommandHttpServers()
    {
        return () =>
            new ConfigCommandTestData
            {
                Servers =
                [
                    new HttpServer
                    {
                        GroupName = "Test Group 1",
                        Instance = "Test 1 Instance",
                        Uri = new Uri("https://example1.com"),
                    },
                    new HttpServer
                    {
                        GroupName = "Test Group 2",
                        Instance = "Test 2 Instance",
                        Uri = new Uri("https://example2.com"),
                    },
                    new HttpServer
                    {
                        GroupName = "Test Group 1",
                        Instance = "Test 3 Instance",
                        Uri = new Uri("https://example3.com"),
                    },
                ],
            };
    }

    public static Func<ConfigCommandTestData> ConfigCommandSqlServerServers()
    {
        return () =>
            new ConfigCommandTestData
            {
                Servers =
                [
                    new SqlServer
                    {
                        GroupName = "Test Group 3",
                        Instance = "Test 3 Instance",
                        ConnectionString =
                            "Data Source=test3.com;Initial Catalog=test3;Integrated Security=True;",
                    },
                    new SqlServer
                    {
                        GroupName = "Test Group 4",
                        Instance = "Test 4 Instance",
                        ConnectionString =
                            "Data Source=test4.com;Initial Catalog=test4;Integrated Security=True;",
                    },
                ],
            };
    }

    public static Func<ConfigCommandTestData> ConfigCommandMixedServers()
    {
        var httpServers = ConfigCommandHttpServers().Invoke();
        var sqlServers = ConfigCommandSqlServerServers().Invoke();
        return () =>
            new ConfigCommandTestData { Servers = [.. httpServers.Servers, .. sqlServers.Servers] };
    }
}
