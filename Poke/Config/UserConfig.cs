namespace Poke.Config;

public record UserConfig : UserConfigV1
{
    public static UserConfig CreateEmpty() => new UserConfig { Version = 1, Servers = [] };
}
