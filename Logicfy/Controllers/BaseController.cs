using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Logicfy.Controllers
{
    public class BaseController : ControllerBase
    {
        protected string GetUserId()
        {
            var claim = User.FindFirst("kullaniciId")
                       ?? User.FindFirst(ClaimTypes.NameIdentifier);

            if (claim == null)
                throw new Exception("Token içinde kullanıcı Id bulunamadı.");

            return claim.Value;
        }
    }
}
