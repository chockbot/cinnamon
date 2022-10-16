namespace Cinnamon.Core
{
    public class ExperienceSetupViewModel
    {
        private bool _isValid = false;
        public List<ScheduleModel> Schedules { get; set; } = new List<ScheduleModel> { new ScheduleModel() };
        public List<ImageCacheModel> Images { get; set; } = new List<ImageCacheModel>();
        public bool IsPrivate { get; set; } = true;

        public List<string> MinimumAgeList { get; set; } = new List<string> {
            "3+", "5+", "8+", "10+", "12+", "18+", "25+", "30+" 
        };
        public bool CanAdultsJoin { get; set; } = false;
        public List<string> LevelOfActivityList { get; set; } = new List<string> { 
            "Sedentary" , "Lightly Active", "Moderately Active", "Very Active"
        };
        public List<string> SkillLevelList { get; set; } = new List<string> { 
            "Novice" , "Advanced Beginner", "Competent", "Proficient", "Expert"
        };
        public Action Changed;
        public string SpecificsYouWillProvide { get; set; }
        public string CustomerBringWithThem { get; set; }
        public string AdditionalRequirements { get; set; }

        public string ActivityLevel { get; set; }
        public string SkillLevel { get; set; }
        public string MinimumAge { get; set; }
        public bool HasErrors { get; set; } = false;

        public long maxFileSize = 2000000;
        public int maxAllowedFiles = 3;
        public bool maxImageUploaded => Images.Count == maxAllowedFiles;

        public bool IsValid
        {
            get { 
                return _isValid; 
            }
            set {
                HasErrors = !value; 
                _isValid = value;
            }     
        }

        public void AddSchedule() {
            Schedules.Add(new ScheduleModel());
        }

        public void RemoveSchedule(ScheduleModel itemToRemove) {
            Schedules.Remove(itemToRemove);
            Changed.Invoke();
        }

        public void AddImage(byte[] imageData) {
            Images.Add(new ImageCacheModel {
                Id = Images.Count + 1,
                ImageData = imageData
            }); 
        }

        public void ValidateForm(ActivityModel activity) {
            // Check If Description is Blank            
            if (String.IsNullOrEmpty(activity.Description)) {
                IsValid = false;
                return;
            }

            // Check Schedule
            foreach (var sched in Schedules) {
                if (String.IsNullOrEmpty(sched.Name) || String.IsNullOrEmpty(sched.DateTime) || sched.Price == 0
                    || String.IsNullOrEmpty(sched.PriceUnit1) || String.IsNullOrEmpty(sched.PriceUnit2)) {
                    IsValid = false;
                    return;
                } 
            }

            if (String.IsNullOrEmpty(SpecificsYouWillProvide)) {
                IsValid = false;
                return;
            }

            if (String.IsNullOrEmpty(CustomerBringWithThem)) {
                IsValid = false;
                return;
            }


            if (String.IsNullOrEmpty(MinimumAge)) {
                IsValid = false;
                return;
            }

            if (String.IsNullOrEmpty(ActivityLevel)) {
                IsValid = false;
                return;
            }

            if (String.IsNullOrEmpty(SkillLevel)) {
                IsValid = false;
                return;
            }


            IsValid = true;
        }
    }
}
