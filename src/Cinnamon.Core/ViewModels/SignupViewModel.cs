using Cinnamon.Core.Models;

namespace Cinnamon.Core.ViewModels
{
    public class SignupViewModel
    {
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public CustomerModel Customer { get; set;}
        public WaitListModel waitListModel = new WaitListModel();

        public UserListModel userListModel = new UserListModel();
        public bool isSubmit { get; set; } = false;
        public bool isClickButton { get; set; } = false;

        public bool HasError { get; set; } = false;

        public bool isValid(string email)
        {
            if(!string.IsNullOrEmpty(email))
            {
                isClickButton = true;
                return true;
            }
            return false;
        }
        public bool isValidPassword(string password, string confirmPassword)
        {
            if (!string.IsNullOrEmpty(password) && !string.IsNullOrEmpty(confirmPassword) && password == confirmPassword)
            {
                isClickButton = true;
                return true;
            }
            return false;
        }
        public async Task<bool> SaveEmail(WaitListModel waitListModel)
        {
            var res = await CoreDI.DataStore.WaitList.SaveDataAsync(waitListModel);
            if (res.Type == MessageType.Success)
            {
                return true;
            }
            isClickButton = false;
            return false;
        }
    }
}
