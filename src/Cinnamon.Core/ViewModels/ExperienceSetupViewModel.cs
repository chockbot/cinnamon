namespace Cinnamon.Core
{
    public class ExperienceSetupViewModel
    {
        private bool _isValid = false;
        public List<ScheduleModel> Schedules { get; set; } = new List<ScheduleModel> { new ScheduleModel { Id = 1} };
        public List<ImageCacheModel> Images { get; set; } = new List<ImageCacheModel> { 
            new ImageCacheModel{ Id = 1 }, new ImageCacheModel{ Id = 2} , new ImageCacheModel{ Id = 3}
        };
        public bool IsPrivate { get; set; } = true;

        public bool CanAdultsJoin { get; set; } = false;
        public List<string> LevelOfActivityList { get; set; } = new List<string> { 
            "Beginner" , "Intermediate", "Advance"
        };
        public List<string> SkillLevelList { get; set; } = new List<string> { 
            "No experience" , "Little experience", "Expert"
        };
        public Action Changed;
        public string SpecificsYouWillProvide { get; set; }
        public string CustomerBringWithThem { get; set; }
        public string AdditionalRequirements { get; set; }

        public string ActivityLevel { get; set; }
        public string SkillLevel { get; set; }
        public string MinimumAge { get; set; }
        public bool HasErrors { get; set; } = false;

        public long maxFileSize = 10000000;
        public int maxAllowedFiles = 3;
        public bool maxImageUploaded { get; set; } = false;
        public int currentImageId => _currentImageId();

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

        private int _currentImageId() {
            foreach (var image in Images) {
                if (image.IsBlank()) {
                    return image.Id; 
                } 
            }
            return 1;
        }

        private void _maxImageUploaded() {
            foreach (var image in Images) {
                if (image.IsBlank()) {
                    maxImageUploaded = false;
                    return;
                }
            }
            maxImageUploaded = true; 
        }

        public void setImage(int imageIndex, byte[] imageData)
        {
            Images[imageIndex].ImageData = imageData; 
            Images[imageIndex].Status = ResultModel.success("");
            _maxImageUploaded();
            Changed.Invoke();
        }

        public void ClearImage(ImageCacheModel image)
        {
            Images[image.Id - 1].ImageData = null;
            Images[image.Id - 1].Status = null;
            _maxImageUploaded();
            Changed.Invoke();
        }

        public void AddSchedule() {
            // Get Next Number
            var maxIdItem = Schedules.MaxBy(x => x.Id);
            var maxId = 0;
            if (maxIdItem != null) {
                maxId = maxIdItem.Id; 
            }
            Schedules.Add(new ScheduleModel { Id = maxId + 1 });
        }

        public void RemoveSchedule(ScheduleModel itemToRemove) {
            Schedules.Remove(itemToRemove);
            Changed.Invoke();
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
