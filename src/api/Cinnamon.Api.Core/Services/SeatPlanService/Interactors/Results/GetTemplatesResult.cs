namespace Cinnamon.Api.Core.Services.SeatPlanService.Interactors.Result;

public class GetTemplatesResult
{
    public IEnumerable<Template> Templates { get; set; }

    public class Template 
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string ImageSrc { get; set; }
        public bool Enabled { get; set; }
        public string Id { get; set; }
        public string SeatPlanFormatterId { get; set; }
        public string UploadedBy { get; set; }
        public string UploadedDate { get; set; }
    }
}