namespace PublicLobby_Server.Api.Servers;

public class Room : IDisposable
{
    public List<Player> Players = [];
    public Player? Host;
    public GameCode? Code { get; set; }
    public RoomStates States { get; set; }
    public GameServer? Server { get; set; }
    public GameVersion? _GameVersion { get; set; }

    public bool Join(Player player)
    {
        return true;
    }
    
    public bool Exit(Player player)
    {
        return true;
    }
    
    public void Dispose()
    {
    }

    public bool Has(Room room)
    {
        return false;
    }
}