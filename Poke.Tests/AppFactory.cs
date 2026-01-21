using Microsoft.Extensions.DependencyInjection;
using Poke.Config;
using Poke.Config.Migrations;
using Poke.Infrastructure;
using Poke.Runners;
using Poke.Tests.Fakes;
using Spectre.Console;
using Spectre.Console.Cli.Testing;
using Spectre.Console.Testing;

namespace Poke.Tests;

public class AppFactory
{
    public static CommandAppTesterSettings TestSettings = new() { TrimConsoleOutput = true };

    public CommandAppTester CreateApp(UserConfig? userConfig = null)
    {
        FakeConfigFile = new FakeConfigFile { Config = userConfig };

        var services = new ServiceCollection();
        var console = new TestConsole();

        services.AddSingleton<IConfigFile>(FakeConfigFile);
        services.AddSingleton<ConfigMigrator>();
        services.AddSingleton<ConfigManager>();
        services.AddSingleton<IConfigOutput, HttpServerConfigOutput>();
        services.AddSingleton<IConfigOutput, SqlServerConfigOutput>();
        services.AddSingleton<IMigration, V1>();
        services.AddSingleton<IMigration, V2>();
        services.AddSingleton<IAnsiConsole>(console);

        var registrar = new TypeRegistrar(services);
        return new CommandAppTester(registrar, TestSettings, console);
    }

    public FakeConfigFile FakeConfigFile { get; private set; } = null!;
}
