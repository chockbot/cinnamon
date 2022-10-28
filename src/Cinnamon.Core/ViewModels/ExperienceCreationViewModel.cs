using Cinnamon.Core.Models;

namespace Cinnamon.Core
{
    public class ExperienceCreationViewModel
    {
        public List<ExperienceCategoryModel> ExperienceCategory { get; set; } = new List<ExperienceCategoryModel>();
        public string Address1 { get; set; } 
        public string Address2 { get; set; }
        public string District { get; set; }
        public string City { get; set; }
        public int ExperienceCategoryId { get; set; }
        public bool HasError { get; set; } = false;

        public ExperienceCreationViewModel()
        {
            _ = GetInitialValues();
        }

        private async Task GetInitialValues() {
            ExperienceCategory = await CoreDI.DataStore.ExperienceCategory.GetAllAsync();
        }

        public bool IsValid(ActivityModel activity) {
            // Address is Required
            if (!AddressDisable(activity.ExperienceTypeId)) {
                // Check Values 
                if (String.IsNullOrEmpty(Address1) || String.IsNullOrEmpty(Address2) || String.IsNullOrEmpty(District) || String.IsNullOrEmpty(City)) {
                    HasError = true;
                    return false;  
                }
            }

            HasError = false;   
            return true; 
        }

        public bool AddressDisable(int experienceType) {
            return experienceType == 2;
        }
    }
}
