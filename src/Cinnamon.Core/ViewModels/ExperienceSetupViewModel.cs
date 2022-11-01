using Cinnamon.Core.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Cinnamon.Core
{
    public class ExperienceSetupViewModel
    {
        private bool _isValid = false;
        public List<ScheduleModel> Schedules { get; set; } = new List<ScheduleModel> { new ScheduleModel { Id = 1} };
        public List<ImageCacheModel> Images { get; set; } = new List<ImageCacheModel> { 
            new ImageCacheModel{ Id = 1 }, new ImageCacheModel{ Id = 2} , new ImageCacheModel{ Id = 3}
        };
        public DescriptionSectionModel DescriptionSection { get; set; } = new DescriptionSectionModel();
        public bool IsPrivate { get; set; } = true;
        public List<string> LevelOfActivityList { get; set; } = new List<string> { 
            "Beginner" , "Intermediate", "Advance"
        };
        public List<string> SkillLevelList { get; set; } = new List<string> { 
            "No experience" , "Little experience", "Expert"
        };
        public Action Changed;
 
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

        public async void ValidateForm(ActivityModel activity) {
            activity.ScheduleList = Schedules;
            activity.DescriptionSectionModel = DescriptionSection;
            if (activity.DescriptionSectionModel == null) {
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

            if (String.IsNullOrEmpty(DescriptionSection.SpecificsYouWillProvide)) {
                IsValid = false;
                return;
            }

            if (String.IsNullOrEmpty(DescriptionSection.CustomerBringWithThem)) {
                IsValid = false;
                return;
            }


            if (String.IsNullOrEmpty(DescriptionSection.MinimumAge)) {
                IsValid = false;
                return;
            }

            if (String.IsNullOrEmpty(DescriptionSection.ActivityLevel)) {
                IsValid = false;
                return;
            }

            if (String.IsNullOrEmpty(DescriptionSection.SkillLevel)) {
                IsValid = false;
                return;
            }

            foreach(var image in Images)
            {
                if(image.IsBlank())
                {
                    IsValid = false;
                    return;
                }
            }

            IsValid = true;
        }
    }
}
