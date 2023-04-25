namespace Cinnamon.Api.Core.Config;

public class Disbursement 
{
    public bool RunDisbursement {get; set;}
    public int RunPerHour {get; set;}
    public string CronString {get; set;}
}