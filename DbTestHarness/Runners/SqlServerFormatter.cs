using DbTestHarness.Infrastructure;
using DbTestHarness.Models;

namespace DbTestHarness.Runners;

public class SqlServerFormatter : IRunnerFormatter
{
    public string FormatInProgressLine(Server server)
    {
        return $"[blue]{server.GroupName}[/] | [yellow]{server.Instance}[/] | {server.Host}";
    }

    public string FormatResultLine(Server server, RunResult result)
    {
        var (color, symbol) = result.Succeeded
            ? ("green", "✔")
            : ("red", "✘");

        return $"[{color}]{symbol}[/] [blue]{server.GroupName}[/] | [yellow]{server.Instance}[/] | {server.Host}";
    }

    public string FormatExceptionLine(Server server)
    {
        return $"[blue]{server.GroupName}[/] | [yellow]{server.Instance}[/] | {server.Host}";
    }
}