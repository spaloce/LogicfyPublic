using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Logicfy.Controllers
{
    public class BaseController : ControllerBase
    {
        protected int GetUserId()
        {
            var id = User?.FindFirstValue(ClaimTypes.NameIdentifier);
            return id != null ? int.Parse(id) : 0;
        }

        protected IActionResult Success(object data)
        {
            return Ok(new { success = true, data });
        }

        protected IActionResult Success(string message)
        {
            return Ok(new { success = true, message });
        }

        protected IActionResult Fail(string message)
        {
            return BadRequest(new { success = false, message });
        }
    }
}
