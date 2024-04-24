namespace PublicLobby_Server.Api.Servers;

public class Player
{
    public string Name { get; set; } = string.Empty;
    public string Puid { get; set; } = string.Empty;
    public string FriendCode { get; set; } = string.Empty;
    public List<Mod> Mods { get; set; } = [];
    public GameVersion? Version { get; set; }
    public int PlayerId { get; set; }
}