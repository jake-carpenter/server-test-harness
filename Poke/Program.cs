using Microsoft.Extensions.DependencyInjection;
using Poke.Commands;
using Poke.Config;
using Poke.Config.Migrations;
using Poke.Infrastructure;
using Poke.Runners;
using Spectre.Console.Cli;

var services = new ServiceCollection();
var registrar = new TypeRegistrar(services);
var app = new CommandApp(registrar);

// DI registrations
services.AddSingleton<IRunner, SqlServerRunner>();
services.AddSingleton<IRunner, HttpServerRunner>();
services.AddSingleton<RunnerFactory>();
services.AddSingleton<RunnerStatus>();
services.AddSingleton<JsonConfigFile>();
services.AddSingleton<ConfigManager>();
services.AddSingleton<ConfigMigrator>();
services.AddSingleton<IConfigOutput, SqlServerConfigOutput>();
services.AddSingleton<IConfigOutput, HttpServerConfigOutput>();
services.AddSingleton<IMigration, V1>();
services.AddSingleton<IMigration, V2>();

app.Configure(cfg =>
{
    cfg.UseAssemblyInformationalVersion();

    cfg.AddCommand<SelectCommand>("select")
        .WithDescription("Display a list of configured servers to execute against.");

    cfg.AddCommand<AllCommand>("all")
        .WithDescription("Run all configured servers without selection.");

    cfg.AddCommand<ConfigCommand>("config").WithDescription("Display configuration.");

#pragma warning disable CS0618 // Type or member is obsolete - kept for backwards compatibility
    cfg.AddCommand<AddCommand>("add")
        .WithDescription(
            "[[Deprecated]] Add a new SQL Server to the configuration file. Use 'new sqlserver' instead."
        );
#pragma warning restore CS0618

    cfg.AddBranch(
        "new",
        branch =>
        {
            branch.SetDescription("Add a new server to the configuration file.");

            branch
                .AddCommand<AddSqlServerCommand>("sqlserver")
                .WithDescription("Add a new SQL Server to the configuration file.");

            branch
                .AddCommand<AddHttpCommand>("http")
                .WithDescription("Add a new HTTP Server to the configuration file.");
        }
    );
});

await app.RunAsync(args);
