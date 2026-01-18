using Poke.Infrastructure;
using Poke.Models;
using Spectre.Console;

namespace Poke.Runners;

/// <summary>
/// Writes SQL Server configuration output to the console.
/// </summary>
public class SqlServerConfigOutput : IConfigOutput
{
    /// <summary>
    /// The server type this output handles.
    /// </summary>
    public string ServerType => "SqlServer";

    /// <summary>
    /// Writes formatted configuration output for SQL Server groups.
    /// </summary>
    /// <param name="groups">The server groups to output.</param>
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
            new Text("Host", headerTextStyle)
        );

        foreach (var group in groups)
        {
            for (var index = 0; index < group.Servers.Length; index++)
            {
                var server = group.Servers[index];
                if (server is not SqlServer sqlServer)
                    continue;

                // Only show the group name on the first server in the group
                var maybeGroupName =
                    index == 0 ? new Text(server.GroupName, new Style(Color.Blue)) : new Text("");

                grid.AddRow(
                    maybeGroupName,
                    new Text(server.Instance),
                    new Text(sqlServer.DataSource)
                );
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
