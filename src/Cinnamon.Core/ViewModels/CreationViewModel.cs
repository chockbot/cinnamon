using Cinnamon.Core.Models;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Text.Json.Nodes;
using static System.Net.Mime.MediaTypeNames;

namespace Cinnamon.Core
{
    public class CreationViewModel
    {
        public ExperienceCreationViewModel ExperienceCreationViewModel { get; set; } = new ExperienceCreationViewModel();
        public ExperienceSetupViewModel ExperienceSetupViewModel { get; set; } = new ExperienceSetupViewModel();    
        private List<CreationStep> creationSteps = new List<CreationStep> {
            CreationStep.Overview,
            CreationStep.ExperienceCreation,
            CreationStep.ExperienceSetup,
            CreationStep.Publish
        };

        private int _currentStepIndex = 0;

        private int currentStepIndex {
            get { return _currentStepIndex;  }
            set {
                PercentageCompletion = Convert.ToInt32(Math.Round(value * 33.33));
                _currentStepIndex = value;
            }
        }

        public int PercentageCompletion { get; set; } = 0;

        public bool IsComplete => currentStepIndex == creationSteps.Count - 1;
        public CreationStep currentStep => creationSteps[currentStepIndex];

        public ActivityModel activity { get; set; } = new ActivityModel();

        public void GotoStep(CreationStep step) {
            // Check if you can jump to next step
            if (!ValidExperienceCreation()) {
                return; 
            }

            // Validate Experience Setup
            if (!ValidateExperienceSetup())
            {
                // Do not go to next step 
                return;
            }

            if (!ValidatePublishSetup())
            {
                return;
            }

            currentStepIndex = creationSteps.IndexOf(step);
        }

        public void NextStep() {
            // Validate Experience Creation 
            if (!ValidExperienceCreation()) {
                // Do no go to next step
                return; 
            }

            // Validate Experience Setup
            if (!ValidateExperienceSetup()) {
                // Do not go to next step 
                return;
            }

            if (!ValidatePublishSetup())
            {
                return;
            }

            if (currentStepIndex < creationSteps.Count - 1)
            {
                currentStepIndex++;
            }
        }

        private bool UploadImage(List<ImageCacheModel> Images)
        {
            try
            {
                foreach (var image in Images)
                {
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"app/wwwroot/images/Activities/", image.ImageName + ".jpg");
                    var fs = File.Create(filePath);
                    fs.Write(image.ImageData, 0, image.ImageData.Length);
                    fs.Close();
                }
                return true;
            }
            catch(Exception ex)
            {
                foreach (var image in Images)
                {
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"app/wwwroot/images/Activities/", image.ImageName + ".jpg");
                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                    }
                }
                return false;
            }
        }

        public async Task<bool> SaveActivity(ActivityModel activityModel)
        {
            var sortPrice = ExperienceSetupViewModel.Schedules.OrderBy(x => x.Price).ToList();
            activityModel.Price = sortPrice.Count > 0 ? String.Format("{0} {1} - {2}",sortPrice.ElementAtOrDefault(0).UnitPrice, sortPrice.ElementAtOrDefault(0).Price, sortPrice.ElementAtOrDefault(sortPrice.Count-1).Price) : String.Format("{0} {1}",sortPrice.ElementAtOrDefault(0).UnitPrice, sortPrice.ElementAtOrDefault(0).Price.ToString());

            if (activityModel.ExperienceTypeId == 1)
            {
                activityModel.Address = ExperienceCreationViewModel.Address;
            }
            else
            {
                activityModel.Address = null;
            }

            var images = new List<ActivityImagesModels>();
            try
            {
                foreach (var image in ExperienceSetupViewModel.Images)
                {
                    var guid = Guid.NewGuid().ToString();
                    images.Add(new ActivityImagesModels()
                    {
                        ImageLocation = "images/Activities/" + guid + ".jpg"
                    });
                    image.ImageName = guid;
                }

                activity.ActivityImages = images;

                var res = await CoreDI.DataStore.Activities.SaveDataAsync(activityModel);
                if (res.Type == MessageType.Success)
                {

                    if (!UploadImage(ExperienceSetupViewModel.Images))
                    {
                        throw new Exception();
                    }
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }

            return false;
        }

        public void PrevStep()
        {
            if (currentStepIndex > 0) {
                currentStepIndex--; 
            }
        }

        private bool ValidExperienceCreation() {
            if (currentStep == CreationStep.ExperienceCreation)
            {
                return ExperienceCreationViewModel.IsValid(activity);
            }

            return true;
        }

        private bool ValidateExperienceSetup() {
            if (currentStep == CreationStep.ExperienceSetup)
            {
                ExperienceSetupViewModel.ValidateForm(activity);
                return ExperienceSetupViewModel.IsValid;
            }

            return true;
        }

        private bool ValidatePublishSetup()
        {
            if (currentStep == CreationStep.Publish)
            {
                ExperienceSetupViewModel.ValidateForm(activity);
                return ExperienceSetupViewModel.IsValid;
            }

            return true;
        }

    }
}
