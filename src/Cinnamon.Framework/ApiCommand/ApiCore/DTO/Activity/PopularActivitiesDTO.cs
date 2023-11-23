namespace Cinnamon.Framework.ApiCommand.ApiCore.DTO.Activity;

public class PopularActivitiesDTO
{
    public IEnumerable<PopularActivity> Activities {get; set;}
    
    public class PopularActivity 
    {
        public int Id {get; set;}
        public int StudentCount {get; set;}
        public int ReviewCount {get; set;}
        public decimal Rating {get; set;}
        public int OngoingStudentCount {get; set;}
        public string Title {get ;set;}
        public int MakerId {get; set;}
        public bool IsNew {get; set;}
        public string Handler {get; set;}
        public int ExperienceTypeId {get; set;}
        public string Price {get; set;}
        public string CityName {get; set;}
        public string RegionName {get; set;}
        public string PinnedLocation { get; set; }
        public string ImageSrc {get; set;}
        public int ExperienceCreationTypeId {get; set;}
        public Cinnamon.Framework.Enums.Enums.ExperienceCreationType ExperienceCreationType { get; set; }
    }
}
