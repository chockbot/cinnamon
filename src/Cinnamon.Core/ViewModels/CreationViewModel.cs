using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

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

        public Cinnamon.Core.Enums.UserActionType UserActionType { get; set; } = Enums.UserActionType.Create;

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

            try
            {
                if(UserActionType == Enums.UserActionType.Create)
                {
                    activity.ActivityImages = await Upload(ExperienceSetupViewModel.Images);
                    var res = await CoreDI.DataStore.Activities.SaveDataAsync(activityModel);
                    if (res.Type == MessageType.Success)
                    {
                        return true;
                    }
                }
                else if(UserActionType == Enums.UserActionType.Update)
                {
                    var res = await CoreDI.UpdateActivityHandler.ExecuteAsync(new Module.ActivityService.Interactors.UpdateActivity {Activity = activityModel});
                    if(!res.Succeeded)
                    {
                       throw res.Error.Exception;
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


        public async Task<List<ActivityImagesModels>> Upload(List<ImageCacheModel> Images)
        {
            try
            {
                var images = new List<ActivityImagesModels>();

                var configuration = new ConfigurationBuilder()
                 .SetBasePath(Directory.GetCurrentDirectory())
                 .AddJsonFile($"appsettings.json");

                var config = configuration.Build();
                var connectionString = config.GetConnectionString("AzureConnectionString");
                foreach (var image in Images)
                {
                    var guid = Guid.NewGuid().ToString();

                    CloudStorageAccount storageacc = CloudStorageAccount.Parse(connectionString);

                    CloudBlobClient blobClient = storageacc.CreateCloudBlobClient();
                    CloudBlobContainer container = blobClient.GetContainerReference("upload-container");

                    CloudBlockBlob blockBlob = container.GetBlockBlobReference(guid + ".jpg");
                    blockBlob.Properties.ContentType = "image/jpg";

                    using (var ms = new MemoryStream(image.ImageData, false))
                    {
                        await blockBlob.UploadFromStreamAsync(ms);
                    }
                    images.Add(new ActivityImagesModels() { ImageLocation = blockBlob.Uri.ToString(), ImageName = guid });
                }
                return images;
            }
            catch (Exception ex)
            {
                return (List<ActivityImagesModels>)Enumerable.Empty<ActivityImagesModels>();
            }
        }

    }
}
