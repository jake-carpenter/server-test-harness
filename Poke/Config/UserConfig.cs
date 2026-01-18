namespace Poke.Config;

/// <summary>
/// The current user configuration model.
/// </summary>
public record UserConfig : UserConfigV1
{
    /// <summary>
    /// Creates an empty configuration at the latest version.
    /// </summary>
    /// <returns>An empty configuration instance.</returns>
    public static UserConfig CreateEmpty() => new UserConfig { Version = 1, Servers = [] };
}
