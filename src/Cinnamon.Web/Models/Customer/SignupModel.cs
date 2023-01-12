using Cinnamon.Web.Models.Entities;

namespace Cinnamon.Web.Models.Customer;

public class SignupModel
{
    public bool IsClickButton {get; set;}
    public string ModalClass {get; set;}
    public string ModalDisplay {get; set;}
    public bool ShowBackdrop {get; set;}

    public string FirstName {get; set;}
    public string LastName {get; set;}
    public string Birthdate {get; set;}
    public string Email {get; set;}
    public string Password {get; set;}
    public string ConfirmPassword {get; set;}
}