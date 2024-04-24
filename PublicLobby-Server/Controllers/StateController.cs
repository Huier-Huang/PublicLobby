using Microsoft.AspNetCore.Mvc;
using PublicLobby_Server.Api.Servers;
using PublicLobby_Server.Managers;

namespace PublicLobby_Server.Controllers;

[ApiController]
public class StateController(RoomManager _roomManager, PlayerManager _playerManager) : ControllerBase
{
    [HttpGet]
    [Route("/api/room/state/get&GameCode={gameCode:int}", Name = "GetState")]
    public IActionResult GetState(int gameCode)
    {
        if (_roomManager.TryGetRoom(GameCode.From(gameCode), out var room))
        {
            return Ok(room!.States);
        }
        return BadRequest();
    }
    
    
    [HttpPost]
    [Route("/api/room/state/set&GameCode={gameCode:int}&State:{roomState}", Name = "SetState")]
    public IActionResult SetState(int gameCode, RoomStates roomState)
    {
        if (!_roomManager.TryGetRoom(GameCode.From(gameCode), out var room)) return BadRequest();
        room!.States = roomState;
        return Ok();
    }
    
    [HttpPut]
    [Route("/api/room/state/create", Name = "CreateRoom")]
    public IActionResult CreateRoom([FromBody] Room room)
    {
        if (_roomManager.TryAddRoom(room))
        {
            return Ok();
        }

        return BadRequest();
    }
    
    [HttpPut]
    [Route("/api/room/state/Remove", Name = "RemoveRoom")]
    public IActionResult RemoveRoom([FromBody] Room room)
    {
        if (_roomManager.TryRemoveRoom(room))
        {
            return Ok();
        }

        return BadRequest();
    }
    
    [HttpPost]
    [Route("/api/room/state/Join&Code={gameCode:int}&PlayerId={PlayerId:int}", Name = "JoinRoom")]
    public IActionResult JoinRoom(int gameCode, int PlayerId)
    {
        if (!_roomManager.TryGetRoom(gameCode, out var room) || !_playerManager.TryGet(PlayerId, out var player))
            return BadRequest();
        if (room!.Join(player!))
        {
            return Ok();
        }

        return BadRequest();
    }
    
    
    [HttpPost]
    [Route("/api/room/state/Exit&Code={gameCode:int}&PlayerId={PlayerId:int}", Name = "ExitRoom")]
    public IActionResult ExitRoom(int gameCode, int PlayerId)
    {
        if (!_roomManager.TryGetRoom(gameCode, out var room) || !_playerManager.TryGet(PlayerId, out var player))
            return BadRequest();
        if (room!.Exit(player!))
        {
            return Ok();
        }

        return BadRequest();
    }
}