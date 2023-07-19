using System.ComponentModel.DataAnnotations;
using Cinnamon.Framework.Interactor;

namespace Cinnamon.Api.Core.Services.ActivityService.Interactors;

public class ValidateCouponCodeArgs : IInteractor
{
    [Required]
    public int ActivityId {get; set;}

    [Required]
    public string CouponCode {get; set;}

    [Required]
    public decimal Amount {get; set;}
}