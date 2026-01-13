using DbTestHarness.Commands;
using DbTestHarness.Infrastructure;
using DbTestHarness.Models;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console.Cli;

var services = new ServiceCollection();
var registrar = new TypeRegistrar(services);
var app = new CommandApp(registrar);

// DI registrations
var userConfig = await UserConfig.FromConfigDirectory();
services.AddSingleton(_ => userConfig);
services.AddSingleton<IRunner, SqlServerRunner>();
services.AddSingleton<IRunner, DryRunner>();
services.AddSingleton<RunnerFactory>();
services.AddSingleton<RunnerStatus>();

app.Configure(cfg =>
{
    cfg.AddCommand<SelectCommand>("select")
        .WithDescription("Display a list of configured servers to execute against.");

    cfg.AddCommand<AllCommand>("all")
        .WithDescription("Run all configured servers without selection.");

    cfg.AddCommand<ConfigCommand>("config")
        .WithDescription("Display configuration.");
});

await app.RunAsync(args);