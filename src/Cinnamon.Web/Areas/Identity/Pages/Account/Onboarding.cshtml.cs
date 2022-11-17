using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;
namespace Cinnamon.Web.Areas.Identity.Pages.Account;

public class Onboarding : PageModel
{
    private readonly SignInManager<IdentityUser> signInManager;

    public Onboarding(SignInManager<IdentityUser> signInManager)
    {
        this.signInManager = signInManager;
    }

    [BindProperty]
    public InputModel Input { get; set;}

    public async Task<IActionResult> OnGetAsync()
    {
        if(User.Identity.IsAuthenticated)
        {
            return Redirect("/explore");
        }
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if(ModelState.IsValid)
        {
            var login = await signInManager.PasswordSignInAsync(Input.Email,Input.Password, true, false);
            if(login.Succeeded)
            {
                return Redirect("/explore");
            } else {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            }
        }
        return Page();
    }

    public class InputModel 
    {
        [Required]
        public string Password { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}