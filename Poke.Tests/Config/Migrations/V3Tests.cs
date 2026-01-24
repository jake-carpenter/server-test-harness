using System.Text.Json;
using Poke.Config.Migrations;

namespace Poke.Tests.Config.Migrations;

public class V3Tests
{
    private readonly V3 _migration = new();

    [Test]
    public async Task From_Returns_2()
    {
        await Assert.That(_migration.From).IsEqualTo(2);
    }

    [Test]
    public async Task To_Returns_3()
    {
        await Assert.That(_migration.To).IsEqualTo(3);
    }

    [Test]
    public async Task Migrate_Updates_Version_To_3()
    {
        var input = JsonDocument.Parse("""{"version": 2, "servers": []}""");

        var result = _migration.Migrate(input);

        var version = result.RootElement.GetProperty("version").GetInt32();
        await Assert.That(version).IsEqualTo(3);
    }

    [Test]
    public async Task Migrate_Adds_Id_To_Server_Without_Id()
    {
        var input = JsonDocument.Parse(
            """
            {
                "version": 2,
                "servers": [
                    {"groupName": "Test", "instance": "Server1"}
                ]
            }
            """
        );

        var result = _migration.Migrate(input);

        var servers = result.RootElement.GetProperty("servers");
        var server = servers.EnumerateArray().First();

        await Assert.That(server.TryGetProperty("id", out var idProperty)).IsTrue();
        await Assert.That(Guid.TryParse(idProperty.GetString(), out _)).IsTrue();
    }

    [Test]
    public async Task Migrate_Preserves_Existing_Id_On_Server()
    {
        var existingId = "existing-id-12345";
        var input = JsonDocument.Parse(
            $$"""
            {
                "version": 2,
                "servers": [
                    {"id": "{{existingId}}", "groupName": "Test", "instance": "Server1"}
                ]
            }
            """
        );

        var result = _migration.Migrate(input);

        var servers = result.RootElement.GetProperty("servers");
        var server = servers.EnumerateArray().First();
        var id = server.GetProperty("id").GetString();

        await Assert.That(id).IsEqualTo(existingId);
    }

    [Test]
    public async Task Migrate_Assigns_Unique_Ids_To_Multiple_Servers()
    {
        var input = JsonDocument.Parse(
            """
            {
                "version": 2,
                "servers": [
                    {"groupName": "Group1", "instance": "Server1"},
                    {"groupName": "Group2", "instance": "Server2"},
                    {"groupName": "Group3", "instance": "Server3"}
                ]
            }
            """
        );

        var result = _migration.Migrate(input);

        var servers = result.RootElement.GetProperty("servers");
        var ids = servers.EnumerateArray().Select(s => s.GetProperty("id").GetString()).ToList();

        await Assert.That(ids.Count).IsEqualTo(3);
        await Assert.That(ids.Distinct().Count()).IsEqualTo(3);
    }

    [Test]
    public async Task Migrate_Handles_Empty_Servers_Array()
    {
        var input = JsonDocument.Parse("""{"version": 2, "servers": []}""");

        var result = _migration.Migrate(input);

        var servers = result.RootElement.GetProperty("servers");
        await Assert.That(servers.GetArrayLength()).IsEqualTo(0);
    }

    [Test]
    public async Task Migrate_Handles_Missing_Servers_Property()
    {
        var input = JsonDocument.Parse("""{"version": 2}""");

        var result = _migration.Migrate(input);

        var version = result.RootElement.GetProperty("version").GetInt32();
        await Assert.That(version).IsEqualTo(3);
        await Assert.That(result.RootElement.TryGetProperty("servers", out _)).IsFalse();
    }

    [Test]
    public async Task Migrate_Preserves_Other_Server_Properties()
    {
        var input = JsonDocument.Parse(
            """
            {
                "version": 2,
                "servers": [
                    {"groupName": "TestGroup", "instance": "TestInstance", "uri": "https://example.com"}
                ]
            }
            """
        );

        var result = _migration.Migrate(input);

        var server = result.RootElement.GetProperty("servers").EnumerateArray().First();

        await Assert.That(server.GetProperty("groupName").GetString()).IsEqualTo("TestGroup");
        await Assert.That(server.GetProperty("instance").GetString()).IsEqualTo("TestInstance");
        await Assert.That(server.GetProperty("uri").GetString()).IsEqualTo("https://example.com");
    }

    [Test]
    public async Task Migrate_Handles_Mixed_Servers_With_And_Without_Ids()
    {
        var existingId = "keep-this-id";
        var input = JsonDocument.Parse(
            $$"""
            {
                "version": 2,
                "servers": [
                    {"id": "{{existingId}}", "groupName": "Group1", "instance": "Server1"},
                    {"groupName": "Group2", "instance": "Server2"}
                ]
            }
            """
        );

        var result = _migration.Migrate(input);

        var servers = result.RootElement.GetProperty("servers").EnumerateArray().ToList();

        await Assert.That(servers[0].GetProperty("id").GetString()).IsEqualTo(existingId);
        await Assert.That(servers[1].TryGetProperty("id", out var newId)).IsTrue();
        await Assert.That(Guid.TryParse(newId.GetString(), out _)).IsTrue();
    }

    [Test]
    public void Migrate_Throws_When_JsonDocument_Is_Null_Content()
    {
        var input = JsonDocument.Parse("null");

        Assert.Throws<InvalidOperationException>(() => _migration.Migrate(input));
    }
}
