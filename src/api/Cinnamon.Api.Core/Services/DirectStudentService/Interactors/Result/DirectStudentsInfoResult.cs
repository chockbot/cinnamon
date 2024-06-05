using Cinnamon.Framework.ApiCommand.ApiCore;
namespace Cinnamon.Api.Core.Services.DirectStudentService.Interactors.Results;

public class DirectStudentsInfoResult
{
    public ErrorInfo? ErrorInfo { get; set; }
    public Pagination? Pagination { get; set; }
    public IEnumerable<DirectStudentInfo> DirectStudentInfos { get; set; }

    public class DirectStudentInfo
    {
        public int Id {get; set;}
        public int ProviderId {get; set;}
        public string Name {get; set;}
        public string Gender {get; set;}
        public string BirthMonth {get; set;}
        public int BirthYear {get; set;}
        public int Age {get; set;}
    }
}
