using Blazorise;
using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Web.Models.Customer;

public class RequestRefundModel 
{
    public FormModel ModelForm {get; set;} = new();
    public string Message {get; set;}
    public Modal ModalRef {get; set;}

    public IEnumerable<EnrolledExperience> EnrolledExperiences {get; set;}
    public int SelectedExperienceId {get; set;}

    public bool IsShowErrorSelectedExperience {get; set;}
    public bool IsShowErrorMessage {get; set;}

    public bool IsSubmitting {get; set;}

    public IList<Requested> RequestedRefunds {get; set;} = new List<Requested>();

    public Validations ValidationsRef {get; set;}

    public class FormModel 
    {
        [Required(ErrorMessage = "Please provide valid reason")]
        public string Reason {get; set;}
    }

    public class EnrolledExperience 
    {
        public int Id {get; set;}
        public string Title {get; set;}
    }

    public class Requested 
    {
        public string ReferenceNo {get; set;}
        public string ExperienceTitle {get; set;}
        public int Status {get; set;}
    }
}