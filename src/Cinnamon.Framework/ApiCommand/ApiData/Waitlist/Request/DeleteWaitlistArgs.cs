using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinnamon.Framework.ApiCommand.ApiData.Waitlist.Request;

public class DeleteWaitlistArgs
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
}
