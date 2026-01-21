using Poke.Commands;
using Poke.Config;
using Poke.Runners;

namespace Poke.Tests.Commands;

[ClassDataSource<AppFactory>()]
public class ConfigCommandTests(AppFactory appFactory)
{
    [Test]
    public async Task ConfigCommand_Returns_Exit_Code_1_When_No_Servers_Configured()
    {
        var config = UserConfig.CreateEmpty();
        var app = appFactory.CreateApp(config);
        app.SetDefaultCommand<ConfigCommand>();

        var result = await app.RunAsync();

        await Assert.That(result.ExitCode).IsEqualTo(1);
        await Assert.That(result.Output).Contains("No servers configured");
    }

    [Test]
    public async Task ConfigCommand_Displays_Error_Message_When_No_Servers_Configured()
    {
        var config = UserConfig.CreateEmpty();
        var app = appFactory.CreateApp(config);
        app.SetDefaultCommand<ConfigCommand>();

        var result = await app.RunAsync();

        await Assert.That(result.ExitCode).IsEqualTo(1);
        await Assert.That(result.Output).Contains("No servers configured");
    }

    [Test]
    [MethodDataSource(typeof(DataSources), nameof(DataSources.ConfigCommandHttpServers))]
    public async Task ConfigCommand_Returns_Exit_Code_0_When_Servers_Configured(ConfigCommandTestData data)
    {
        var config = UserConfig.CreateEmpty() with { Servers = data.Servers };

        var app = appFactory.CreateApp(config);
        app.SetDefaultCommand<ConfigCommand>();

        var result = await app.RunAsync();

        await Assert.That(result.ExitCode).IsEqualTo(0);
    }

    [Test]
    [MethodDataSource(typeof(DataSources), nameof(DataSources.ConfigCommandHttpServers))]
    public async Task ConfigCommand_Displays_Http_Servers_Header_When_Configured(ConfigCommandTestData data)
    {
        var config = UserConfig.CreateEmpty() with { Servers = data.Servers };

        var app = appFactory.CreateApp(config);
        app.SetDefaultCommand<ConfigCommand>();

        var result = await app.RunAsync();
        var expectedServer = data.Servers.Cast<HttpServer>().First();

        await Assert.That(result.Output).Contains("HTTP Server connections");
        await Assert.That(result.Output).Contains(expectedServer.GroupName);
        await Assert.That(result.Output).Contains(expectedServer.Instance);
        await Assert.That(result.Output).Contains(expectedServer.Uri.ToString());
    }

    [Test]
    [MethodDataSource(typeof(DataSources), nameof(DataSources.ConfigCommandHttpServers))]
    public async Task ConfigCommand_Displays_All_Http_Servers_When_Configured(ConfigCommandTestData data)
    {
        var config = UserConfig.CreateEmpty() with { Servers = data.Servers };

        var app = appFactory.CreateApp(config);
        app.SetDefaultCommand<ConfigCommand>();

        var result = await app.RunAsync();

        await Verify(result.Output);
    }

    [Test]
    [MethodDataSource(typeof(DataSources), nameof(DataSources.ConfigCommandSqlServerServers))]
    public async Task ConfigCommand_Displays_SqlServer_Servers_Header_When_Configured(ConfigCommandTestData data)
    {
        var config = UserConfig.CreateEmpty() with { Servers = data.Servers };
        var app = appFactory.CreateApp(config);
        app.SetDefaultCommand<ConfigCommand>();

        var result = await app.RunAsync();

        await Assert.That(result.Output).Contains("SQL Server connections");
    }

    [Test]
    [MethodDataSource(typeof(DataSources), nameof(DataSources.ConfigCommandSqlServerServers))]
    public async Task ConfigCommand_Displays_All_SqlServer_Servers_When_Configured(ConfigCommandTestData data)
    {
        var config = UserConfig.CreateEmpty() with { Servers = data.Servers };
        var app = appFactory.CreateApp(config);
        app.SetDefaultCommand<ConfigCommand>();

        var result = await app.RunAsync();

        await Verify(result.Output);
    }

    [Test]
    [MethodDataSource(typeof(DataSources), nameof(DataSources.ConfigCommandMixedServers))]
    public async Task ConfigCommand_Displays_Mixed_Servers_Header_When_Configured(ConfigCommandTestData data)
    {
        var config = UserConfig.CreateEmpty() with { Servers = data.Servers };
        var app = appFactory.CreateApp(config);
        app.SetDefaultCommand<ConfigCommand>();

        var result = await app.RunAsync();

        await Assert.That(result.Output).Contains("HTTP Server connections");
        await Assert.That(result.Output).Contains("SQL Server connections");
    }

    [Test]
    [MethodDataSource(typeof(DataSources), nameof(DataSources.ConfigCommandMixedServers))]
    public async Task ConfigCommand_Displays_All_Mixed_Servers_When_Configured(ConfigCommandTestData data)
    {
        var config = UserConfig.CreateEmpty() with { Servers = data.Servers };
        var app = appFactory.CreateApp(config);
        app.SetDefaultCommand<ConfigCommand>();

        var result = await app.RunAsync();

        await Verify(result.Output);
    }

    [Test]
    [MethodDataSource(typeof(DataSources), nameof(DataSources.ConfigCommandMixedServers))]
    public async Task ConfigCommand_Uses_Specified_Config_File_Path_When_Provided(ConfigCommandTestData data)
    {
        const string customConfigPath = "/custom/path/config.json";
        var app = appFactory.CreateApp();
        appFactory.FakeConfigFile.Config = UserConfig.CreateEmpty() with { Servers = data.Servers };
        app.SetDefaultCommand<ConfigCommand>();

        await app.RunAsync(["--config", customConfigPath]);

        await Assert.That(appFactory.FakeConfigFile.LastReadFilePath).IsEqualTo(customConfigPath);
    }

    [Test]
    [MethodDataSource(typeof(DataSources), nameof(DataSources.ConfigCommandMixedServers))]
    public async Task ConfigCommand_Uses_Default_Config_File_Path_When_Option_Not_Provided(ConfigCommandTestData data)
    {
        var config = UserConfig.CreateEmpty() with { Servers = data.Servers };
        var app = appFactory.CreateApp(config);
        app.SetDefaultCommand<ConfigCommand>();

        await app.RunAsync();

        using var _ = Assert.Multiple();
        await Assert.That(appFactory.FakeConfigFile.ReadCallCount).IsGreaterThan(0);
        await Assert.That(appFactory.FakeConfigFile.LastReadFilePath).IsNull();
    }
}
