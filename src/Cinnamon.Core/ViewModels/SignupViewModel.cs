using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinnamon.Core.ViewModels
{
    public class SignupViewModel
    {
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

        public async Task<bool> SaveEmail(WaitListModel waitListModel)
        {
            var res = await CoreDI.DataStore.WaitList.SaveDataAsync(waitListModel);
            if (res.Message.Contains("Saved"))
            {
                return true;
            }
            isClickButton = false;
            return false;
        }
    }
}
