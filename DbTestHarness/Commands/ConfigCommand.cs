using DbTestHarness.Infrastructure;
using DbTestHarness.Models;
using Spectre.Console.Cli;

namespace DbTestHarness.Commands;

public class ConfigCommand(IEnumerable<IConfigOutput> writers) : Command<BaseSettings>
{
    public override int Execute(CommandContext context, BaseSettings settings, CancellationToken cancellationToken)
    {
        var config = settings.GetConfig();
        var serverGroups = new ServerGroups(config.Servers);

        foreach (var writer in writers)
        {
            writer.Write(serverGroups.ByType(writer.ServerType));
        }

        return 0;
    }
}