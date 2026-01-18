using Poke.Config;
using Poke.Infrastructure;
using Poke.Models;
using Spectre.Console.Cli;

namespace Poke.Commands;

public class ConfigCommand(IEnumerable<IConfigOutput> writers, ConfigManager configManager)
    : AsyncCommand<BaseSettings>
{
    public override async Task<int> ExecuteAsync(
        CommandContext context,
        BaseSettings settings,
        CancellationToken cancellationToken
    )
    {
        var config = await configManager.Read(settings.ConfigFile);
        var serverGroups = new ServerGroups(config.Servers);

        foreach (var writer in writers)
        {
            writer.Write(serverGroups.ByType(writer.ServerType));
        }

        return 0;
    }
}
