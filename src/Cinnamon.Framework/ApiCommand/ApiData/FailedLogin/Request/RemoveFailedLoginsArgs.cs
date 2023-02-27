using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiData.FailedLogin.Request;

public class RemoveFailedLoginsArgs
{
    [Required]
    public IEnumerable<RemoveFailedLogin> RemoveFailedLogins {get; set;}
    
    public class RemoveFailedLogin 
    {
        [Required]
        public int Id {get; set;}
    }
}