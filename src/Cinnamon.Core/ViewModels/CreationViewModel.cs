namespace Cinnamon.Core
{
    public class CreationViewModel
    {
        private List<CreationStep> creationSteps = new List<CreationStep> {
            CreationStep.Overview, 
            CreationStep.ExperienceCreation,
            CreationStep.ExperienceSetup,
            CreationStep.Publish
        };

        private int currentStepIndex { get; set; } = 0;

        public int PercentageCompletion { get; set; } = 0;

        public bool IsLastStep => currentStepIndex == creationSteps.Count - 1;

        public CreationStep currentStep => creationSteps[currentStepIndex];

        public ActivityModel activity { get; set; } = new ActivityModel();

        public void GotoStep(CreationStep step) {
            currentStepIndex = creationSteps.IndexOf(step);
        }

        public void NextStep() {
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
        
    }
}
