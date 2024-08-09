using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiCore.System.Request;
   
public class SubscribeToMailchimpArgs
{
    [EmailAddress]
    public string EmailAddress { get; set; }
}