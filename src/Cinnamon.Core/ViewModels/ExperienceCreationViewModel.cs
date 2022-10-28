using Cinnamon.Core.Models;

namespace Cinnamon.Core
{
    public class ExperienceCreationViewModel
    {
        public List<ExperienceCategoryModel> ExperienceCategory { get; set; } = new List<ExperienceCategoryModel>();
        public AddressModel Address { get; set; } = new AddressModel();
        public List<string> SearchTags { get; set; } = new List<string>();
        public bool HasError { get; set; } = false;

        public ExperienceCreationViewModel()
        {
            _ = GetInitialValues();
        }

        private async Task GetInitialValues() {
            ExperienceCategory = await CoreDI.DataStore.ExperienceCategory.GetAllAsync();
        }

        public bool IsValid(ActivityModel activity) {
            activity.SearchTagsModel = new SearchTagsModel()
            {
                SearchTag1 = SearchTags.ElementAtOrDefault(0),
                SearchTag2 = SearchTags.ElementAtOrDefault(1),
                SearchTag3 = SearchTags.ElementAtOrDefault(2),
                SearchTag4 = SearchTags.ElementAtOrDefault(3),
                SearchTag5 = SearchTags.ElementAtOrDefault(4)
            };
            // Address is Required
            if (!AddressDisable(activity.ExperienceTypeId)) {
                // Check Values 
                if (String.IsNullOrEmpty(Address.Address1) || String.IsNullOrEmpty(Address.Address2) || String.IsNullOrEmpty(Address.District) || String.IsNullOrEmpty(Address.City)) {
                    HasError = true;
                    return false;  
                }
            }

            if(String.IsNullOrEmpty(activity.Title))
            {
                HasError = true;
                return false;
            }

            if(activity.ExperienceCategoryId == null)
            {
                HasError = true;
                return false;
            }

            if(String.IsNullOrEmpty(activity.SearchTagsModel.SearchTag1))
            {
                HasError = true;
                return false;
            }

            HasError = false;   
            return true; 
        }

        public bool AddressDisable(int experienceType) {
            return experienceType == 2;
        }
    }
}
