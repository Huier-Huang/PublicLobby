using PublicLobby_Server.Api.Servers;

namespace PublicLobby_Server.Managers;

public class RoomManager
{
    public int RoomCount => _Rooms.Count;

    public readonly List<Room> _Rooms = [];
    public readonly List<GameServer> _GameServers = [];

    public bool TryAddRoom(Room room)
    {
        if (_Rooms.Exists(room.Has))
            return false;
        
        _Rooms.Add(room);
        return true;
    }

    // 删除房间
    public bool TryRemoveRoom(Room room)
    {
        if (!_Rooms.Contains(room))
            return false;
        
        room.Dispose();
        _Rooms.Remove(room);
        return true;
    }

    public bool TryGetRoom(GameCode gameCode, out Room? room)
    {
        if (!_Rooms.Exists(n => n.Code! == gameCode))
        {
            room = null;
            return false;
        }

        room = _Rooms.First(n => n.Code! == gameCode);
        return true;
    }
}