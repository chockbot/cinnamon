using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Cinnamon.Web.Models.Account;
using Cinnamon.Core;
using Cinnamon.Core.Module.CinnamonMakerService.Handler;

namespace Cinnamon.Web.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class AccountController : Controller 
{
    private readonly SignInManager<IdentityUser> signInManager;
    private readonly ISubmitWaitngList submitWaitingListHandler;

    public AccountController(SignInManager<IdentityUser> signInManager, ISubmitWaitngList submitWaitingListHandler)
    {
        this.signInManager = signInManager;
        this.submitWaitingListHandler = submitWaitingListHandler;
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
                return Json(new { success = false, message = "Please provide valid email or password" });
            }

            return Json(new { success = true, message = "Successfully login" });
        }
        catch
        {
            return Json(new { success = false, message = "An error occured please try again later" });
        }
    }

    [Route("register")]
    [HttpPost]
    public async Task<IActionResult> Register(RegisterModel model)
    {
        try
        {
            if(!ModelState.IsValid)
            {
                return Json(new { success = false, message = "Please provide valid emai" });
            }

            var waitListRes = await CoreDI.DataStore.WaitList.GetWaitListByEmail(model.Email);
            if(waitListRes != null && waitListRes.IsVerified)
            {
                return Json(new { success = true, message = "Email already verified", code = "VERIFIED" });
            }
            else if(waitListRes != null && !waitListRes.IsVerified)
            {
                return Json(new { success = true, message = "Email already verified", code = "NOTVERIFIED" });
            }
            else 
            {
                var register = await submitWaitingListHandler.ExecuteAsync
                    (new Core.Module.CinnamonMakerService.Interactors.SubmitWaitingList { Email = model.Email });
                
                if(!register.Succeeded)
                {
                    return Json(new { success = false, message = "An error occured please try again later" });
                }

                return Json(new { success = true, message = "Please confirm your email to proceed.", code = "EMAILREGISTERED" });
            }
        }
        catch
        {
            return Json(new { success = false, message = "An error occured please try again later" });
        }
    }
}