using System.Text.Json;

namespace Poke.Config.Migrations;

/// <summary>
/// A migration interface for the configuration file.
/// </summary>
public interface IMigration
{
    /// <summary>
    /// The version from which the migration is applied.
    /// </summary>
    int From { get; }

    /// <summary>
    /// The version to which the migration is applied.
    /// </summary>
    int To { get; }

    /// <summary>
    /// Migrates the configuration file from the current version to the new version.
    /// </summary>
    /// <param name="jsonDocument">The JSON document to migrate.</param>
    /// <returns>The migrated JSON document.</returns>
    JsonDocument Migrate(JsonDocument jsonDocument);
}
