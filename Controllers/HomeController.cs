using Microsoft.AspNetCore.Mvc;

namespace YahooDiscordClient.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet("~/")]
        public ActionResult Index() => View();
    }
}