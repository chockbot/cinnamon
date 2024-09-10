using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiData.OteTicket.Request;

public class CreateOteTicketArgs 
{
    [Required]
    public int ActivityId {get; set;}

    [Required]
    public int OteScheduleId {get; set;}

    [Required]
    public int OteSchedulePricingId {get; set;}

    [Required]
    public int CustomerId {get; set;}

    [Required]
    public int PurchaseOrderId {get; set;}

    [Required]
    public string Title {get; set;}

    [Required]
    public decimal Amount {get; set;}

    [Required]
    public string QRCode {get; set;}

    [Required]
    public string QRImageData {get; set;}

    [Required]
    public string Status {get; set;}

    [Required]
    public int OteDateId {get; set;}

    public string SeatNumber { get; set; }
}