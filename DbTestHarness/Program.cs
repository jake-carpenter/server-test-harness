using DbTestHarness;
using DbTestHarness.Commands;
using DbTestHarness.Infrastructure;
using DbTestHarness.Models;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console.Cli;

var services = new ServiceCollection();
var registrar = new TypeRegistrar(services);
var app = new CommandApp(registrar);

// DI registrations
var userConfig = await UserConfig.FromFile("config.json");
services.AddSingleton<UserConfig>(_ => userConfig);
services.AddSingleton<SqlServerRunner>();
services.AddSingleton<DryRunner>();

app.Configure(cfg =>
{
    cfg.AddCommand<SelectCommand>("select")
        .WithDescription("testing");
});

await app.RunAsync(args);