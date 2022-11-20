using Cinnamon.Core.DI.Interfaces.Tables;
using Cinnamon.Core.Models;
using static System.Net.Mime.MediaTypeNames;

namespace Cinnamon.Core
{
    public class UploadProfileViewModel
    {
        
        public long     maxFileSize         = 10000000;
        public int      maxAllowedFiles     = 1;
        public string   ModalDisplay        = "none;";
        public string   ModalClass          = "";
        public string   profilepicture      = "images/Profile/user.png";
        public bool     ShowBackdrop        = false;
        public bool     isClickButton       = false;
        public bool     isLoading           = false;
    }
    //public async Task SaveImageLocation()
    //{
    //    await 
    //}
}
   

