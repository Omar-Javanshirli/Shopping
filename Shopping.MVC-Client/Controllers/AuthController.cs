using Microsoft.AspNetCore.Mvc;

namespace Shopping.MVC_Client.Controllers
{
    public class AuthController : Controller
    {
      

        public IActionResult Login()
        {
            return View();
        }
    }
}
