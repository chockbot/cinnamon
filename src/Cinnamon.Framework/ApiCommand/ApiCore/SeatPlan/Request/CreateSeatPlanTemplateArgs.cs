using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Cinnamon.Framework.ApiCommand.ApiCore.SeatPlan.Request;

public class CreateSeatPlanTemplateArgs 
{
    [Required]
    public IFormFile JsonFile {get; set;}

    [Required]
    public IFormFile ImageFile {get; set;}

    [Required]
    public string Name {get; set;}

    [Required]
    public string Address {get; set;}

    [Required]
    public int FormatterId {get; set;}
}