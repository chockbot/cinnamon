namespace Cinnamon.Api.Core.Services.ActivityService.Interactors.Results;

public class ProviderQuestionsResult 
{
    public IEnumerable<Question> Questions {get; set;}
    
    public class Question 
    {
        public int Id {get; set;}
        public int ActivitId {get; set;}
        public int ProviderId {get; set;}
        public string FieldLabel {get; set;}
        public string FieldType {get; set;}
        public bool Required {get; set;}
        public string Options { get; set; }
    }
}