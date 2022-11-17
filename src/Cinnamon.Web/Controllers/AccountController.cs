using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Cinnamon.Web.Models.Account;

namespace Cinnamon.Web.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class AccountController : Controller 
{
    private readonly SignInManager<IdentityUser> signInManager;

    public AccountController(SignInManager<IdentityUser> signInManager)
    {
        this.signInManager = signInManager;
    }

    [Route("login")]
    [HttpPost]
    public async Task<IActionResult> Login(LoginModel model)
    {
        try
        {
            if(!ModelState.IsValid)
            {
                return Json(new { success = false, message = "Please provide valid email or password" });
            }
            var login = await signInManager.PasswordSignInAsync(model.Email,model.Password, true, false);
            if(login.IsLockedOut)
            {
                return  Json(new { success = false, message = "You account was locked" });
            }
            if(!login.Succeeded)
            {
                return Json(new { success = false, message = "An error occured please try again later" });
            }

            return Json(new { success = true, message = "Successfully login" });
        }
        catch
        {
            return Json(new { success = false, message = "An error occured please try again later" });
        }
    }
}