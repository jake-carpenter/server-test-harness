using Poke.Commands;
using Poke.Infrastructure;
using Poke.Runners;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console.Cli;

var services = new ServiceCollection();
var registrar = new TypeRegistrar(services);
var app = new CommandApp(registrar);

// DI registrations
services.AddSingleton<IRunner, SqlServerRunner>();
services.AddSingleton<RunnerFactory>();
services.AddSingleton<RunnerStatus>();
services.AddSingleton<UserConfigManager>();
services.AddSingleton<IConfigOutput, SqlServerConfigOutput>();

app.Configure(cfg =>
{
    cfg.AddCommand<SelectCommand>("select")
        .WithDescription("Display a list of configured servers to execute against.");

    cfg.AddCommand<AllCommand>("all")
        .WithDescription("Run all configured servers without selection.");

    cfg.AddCommand<ConfigCommand>("config")
        .WithDescription("Display configuration.");

    cfg.AddCommand<AddCommand>("add")
        .WithDescription("Add a new SQL Server to the configuration file.");
});

await app.RunAsync(args);
