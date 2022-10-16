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
            CreationStep.Publish,
            CreationStep.Complete
        };

        private int _currentStepIndex = 2;

        private int currentStepIndex {
            get { return _currentStepIndex;  }
            set {
                PercentageCompletion = value * 25;
                _currentStepIndex = value;
            }
        }

        public int PercentageCompletion { get; set; } = 0;

        public bool IsLastStep => currentStepIndex == creationSteps.Count - 2;
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

            if (currentStepIndex < creationSteps.Count - 1)
            {
                currentStepIndex++;
            }
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
        
    }
}
