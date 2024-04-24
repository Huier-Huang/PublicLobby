using PublicLobby_Server.Api;
using PublicLobby_Server.Api.Servers;

namespace PublicLobby_Server.Managers;

public class PlayerManager
{
    private int LastId = 0;

    private readonly List<Player> _players = [];
    
    public void AddPlayer(Player player)
    {
        
    }

    public bool TryGet(string PUID, string FriendCode, out Player? getPlayer)
    {
        var player = _players.FirstOrDefault(n => n.Puid == PUID && n.FriendCode == FriendCode);
        if (player != null)
        {
            getPlayer = player;
            return true;
        }
        getPlayer = null;
        return false;
    }
    
    public bool TryGet(int playerId, out Player? getPlayer)
    {
        var player = _players.FirstOrDefault(n => n.PlayerId == playerId);
        if (player != null)
        {
            getPlayer = player;
            return true;
        }
        getPlayer = null;
        return false;
    }

    public int GeneratePlayerId()
    {
        find:
        if (_players.All(n => n.PlayerId != LastId)) return LastId;
        LastId++;
        goto find;
    }


    public Player GeneratePlayer(string PUID, string FriendCode, string Name, GameVersion version)
    {
        return new Player
        {
            Version = version,
            Puid = PUID,
            FriendCode = FriendCode,
            Name = Name,
            PlayerId = GeneratePlayerId()
        };
    }
}