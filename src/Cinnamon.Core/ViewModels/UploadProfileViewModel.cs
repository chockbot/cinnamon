using Cinnamon.Core.DI.Interfaces.Tables;
using Cinnamon.Core.Models;
using static System.Net.Mime.MediaTypeNames;

namespace Cinnamon.Core
{
    public class UploadProfileViewModel
    {
        
        public long     maxFileSize    { get; set; }     = 10000000;
        public int      maxAllowedFiles{ get; set; }     = 1;
        public string   ModalDisplay   { get; set; }     = "none;";
        public string   ModalClass     { get; set; }     = "";
        public string   profilepicture { get; set; }
        public bool     isSubmit       { get; set; }     = false;
        public bool     ShowBackdrop   { get; set; }     = false;
        public bool     isClickButton  { get; set; }     = false;
        public bool     isSpinnerShow  { get; set; }     = false;
        public bool     isLoading      { get; set; }     = false;
        public bool ShowErrorMessage { get; set; }       = false;
        public bool ShowSuccessMessage { get; set; }     = false;
        public string   UserEmail      { get; set; }


    }
    //public async Task SaveImageLocation()
    //{
    //    await 
    //}
}
   

