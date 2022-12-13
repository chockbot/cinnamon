using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class AccountController : ControllerBase 
{
    [Route("/")]
    [HttpGet]
    public IActionResult Index()
    {
        return new JsonResult(new {success = true, });
    }
}