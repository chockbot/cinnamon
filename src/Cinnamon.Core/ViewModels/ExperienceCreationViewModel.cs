using Cinnamon.Core.Models;

namespace Cinnamon.Core
{
    public class ExperienceCreationViewModel
    {
        public List<ExperienceCategoryModel> ExperienceCategory { get; set; } = new List<ExperienceCategoryModel>();
        public AddressModel Address { get; set; } = new AddressModel();
        public List<SearchTag> SearchTags { get; set; } = new List<SearchTag>();
        public bool HasError { get; set; } = false;

        public class SearchTag {
            public int Key { get; set; }
            public string Value { get; set; }
        }
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
                SearchTag1 = SearchTags?.ElementAtOrDefault(0)?.Value,
                SearchTag2 = SearchTags?.ElementAtOrDefault(1)?.Value,
                SearchTag3 = SearchTags?.ElementAtOrDefault(2)?.Value,
                SearchTag4 = SearchTags?.ElementAtOrDefault(3)?.Value,
                SearchTag5 = SearchTags?.ElementAtOrDefault(4)?.Value
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
