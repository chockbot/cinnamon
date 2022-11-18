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
    private readonly UserManager<IdentityUser> userManager;
    private readonly ISubmitWaitngList submitWaitingListHandler;
    private readonly IRegisterMaker registerMaker;

    public AccountController(SignInManager<IdentityUser> signInManager, ISubmitWaitngList submitWaitingListHandler,
        UserManager<IdentityUser> userManager, IRegisterMaker registerMaker)
    {
        this.signInManager = signInManager;
        this.submitWaitingListHandler = submitWaitingListHandler;
        this.userManager = userManager;
        this.registerMaker = registerMaker;
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
                return Json(new { success = false, message = "Please provide valid email" });
            }

            var waitListRes = await CoreDI.DataStore.WaitList.GetWaitListByEmail(model.Email);
            if(waitListRes != null && waitListRes.IsVerified)
            {
                // check if email already registered
                var res = await userManager.FindByEmailAsync(model.Email);
                if(res == null)
                {
                    return Json(new { success = true, message = "Email already verified but not yet registered", code = "NOTREGISTERED" });
                }
                return Json(new { success = true, message = "Email already verified", code = "VERIFIED" });
            }
            else if(waitListRes != null && !waitListRes.IsVerified)
            {
                return Json(new { success = true, message = "Email not yet verified", code = "NOTVERIFIED" });
            }
            else 
            {
                var register = await submitWaitingListHandler.ExecuteAsync
                    (new Core.Module.CinnamonMakerService.Interactors.SubmitWaitingList { Email = model.Email });
                
                if(!register.Succeeded)
                {
                    return Json(new { success = false, message = "An error occured please try again later", code = "ERRORREGISTER" });
                }

                return Json(new { success = true, message = "Please confirm your email to proceed.", code = "EMAILREGISTERED" });
            }
        }
        catch
        {
            return Json(new { success = false, message = "An error occured please try again later" });
        }
    }

    [Route("registerautologin")]
    [HttpPost]
    public async Task<IActionResult> RegisterAutoLogin(RegisterAutoLogin model)
    {
        try
        {
            if(!ModelState.IsValid)
            {
                return Json(new { success = false, message = "Please provide required fields" });
            }

            // register customer information
            var register = await registerMaker.ExecuteAsync(new Core.Module.CinnamonMakerService.Interactors.RegisterMaker {
                AcceptFlag = model.AcceptFlag,
                Birthdate = model.Birthdate,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Password = model.Password
            });

            if(!register.Succeeded)
            {
                return Json(new { success = false, message = "An error occured please try again later" });
            }

            // auto login
            var login = await signInManager.PasswordSignInAsync(model.Email,model.Password, true, false);
            if(!login.Succeeded)
            {
                return Json(new { success = false, message = "An error occured please try again later" });
            }

            return Json(new { success = true, message = "Successfully registered" });
        }
        catch 
        {
            return Json(new { success = false, message = "An error occured please try again later" });
        }
    }
}