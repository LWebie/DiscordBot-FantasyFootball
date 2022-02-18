using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace YahooDiscordClient.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        public IActionResult Login(string returnUrl = "/")
        {
            return Challenge(new AuthenticationProperties() { RedirectUri = returnUrl });
        }
    }
}