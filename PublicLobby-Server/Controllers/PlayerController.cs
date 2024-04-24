using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PublicLobby_Server.Api.Servers;
using PublicLobby_Server.Managers;

namespace PublicLobby_Server.Controllers;


[Route("/api/player",Name = "Player")]
[ApiController]
public class PlayerController(IOptions<ServerConfig> _config, RoomManager _roomManager, PlayerManager _playerManager) : ControllerBase
{
    [HttpPut]
    public IActionResult PutServer()
    {
        var info = new ApiServerInfo
        {
            Ip = _config.Value.ListenIp,
            Port = _config.Value.ListenPort,
            RoomCount = _roomManager.RoomCount
        };
        return Ok(JsonSerializer.Serialize(info));
    }

    [Route("/api/player/create&{friendCode}&{Puid}&{Name}&{Version:int}" , Name = "CreatePlayer")]
    [HttpPost]
    public IActionResult PostCreate(string friendCode, string Puid, string Name, int Version)
    {
        Player ReturnPlayer;
        var version = new GameVersion(Version);
        if (_playerManager.TryGet(Puid, friendCode, out var player))
        {
            player!.Name = Name;
            player.Version = version;

            ReturnPlayer = player;
        }
        else
        {
            ReturnPlayer = _playerManager.GeneratePlayer(Puid, friendCode, Name, version);
            _playerManager.AddPlayer(ReturnPlayer);
        }
        
        return Ok(JsonSerializer.Serialize(ReturnPlayer));
    }

    private class ApiServerInfo
    {
        public string Ip { get; set; } = null!;
        public int Port { get; set; }
        public int RoomCount { get; set; }
    }
}