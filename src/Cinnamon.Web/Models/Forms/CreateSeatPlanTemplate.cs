using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Web.Models.Forms;

public class CreateSeatPlanTemplate 
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