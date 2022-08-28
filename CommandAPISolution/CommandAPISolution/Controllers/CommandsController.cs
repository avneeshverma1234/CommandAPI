using System.Dynamic;
using Microsoft.AspNetCore.Mvc;

namespace CommandAPISolution.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CommandsController: ControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<string>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<IEnumerable<string>> Get()
    {
        return new string[] {"this", "is", "hard", "coded"};
    }
}