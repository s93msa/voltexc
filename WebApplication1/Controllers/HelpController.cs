using Microsoft.AspNetCore.Mvc;

namespace VoltigeCore.Controllers
{
    public class HelpController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
