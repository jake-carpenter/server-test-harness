using DbTestHarness.Infrastructure;
using DbTestHarness.Models;
using Spectre.Console;

namespace DbTestHarness.Runners;

public class SqlServerConfigOutput : IConfigOutput
{
    public string ServerType => "SqlServer";

    public void Write(IEnumerable<ServerGroup> groups)
    {
        var headerTextStyle = new Style(Color.Grey);
        var grid = new Grid() { Expand = true };

        grid.AddColumn(new GridColumn().Alignment(Justify.Right));
        grid.AddColumn(new GridColumn().Padding(4, 0));
        grid.AddColumn(new GridColumn());

        grid.AddRow(
            new Text("Group", headerTextStyle),
            new Text("Instance", headerTextStyle),
            new Text("Host", headerTextStyle));

        foreach (var group in groups)
        {
            for (var index = 0; index < group.Servers.Length; index++)
            {
                var server = group.Servers[index];

                // Only show the group name on the first server in the group
                var maybeGroupName = index == 0 ? new Text(server.GroupName, new Style(Color.Blue)) : new Text("");

                grid.AddRow(
                    maybeGroupName,
                    new Text(server.Instance),
                    new Text(server.Host));
            }
        }

        var panel = new Panel(grid)
            .BorderColor(Color.Yellow)
            .RoundedBorder()
            .Header("SQL Server connections")
            .Expand();


        AnsiConsole.Write(panel);
    }
}

