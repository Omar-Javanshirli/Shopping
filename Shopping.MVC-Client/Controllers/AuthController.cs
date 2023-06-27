using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Shopping.Core.Models.JWTDbModels;
using Shopping.Core.Services;

namespace Shopping.MVC_Client.Controllers
{
    public class AuthController : Controller
    {

        private readonly Shopping.Core.Services.IAuthenticationService authenticationService;

        public AuthController(Shopping.Core.Services.IAuthenticationService authenticationService)
        {
            this.authenticationService = authenticationService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(SignInInput request,string returnUrl=null)
        {
            if (ModelState is { IsValid: false })
                return View();
            returnUrl ??= Url.Action("Index", "Home");

            var response= await  this.authenticationService.SignInAsync(request);
            
            if(response is { IsSuccessful: false })
            {
                response.Errors.ForEach(error =>
                {
                    ModelState.AddModelError(String.Empty, error);
                });
                return View();
            }
          

            return RedirectToAction(nameof(Index), "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
    }
}
