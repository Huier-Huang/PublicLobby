using Microsoft.AspNetCore.Mvc;

namespace PublicLobby_Server.Controllers;

[Route("/api/list", Name = "List")]
[ApiController]
public class ListController : ControllerBase
{
    [HttpGet]
    public IActionResult GetList()
    {
        return Ok();
    }
}