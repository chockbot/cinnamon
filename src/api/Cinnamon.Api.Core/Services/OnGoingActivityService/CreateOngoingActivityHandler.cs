using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Services.AccountService.Handlers;
using Cinnamon.Api.Core.Services.OngoingActivityService.Handlers;
using Cinnamon.Api.Core.Services.OngoingActivityService.Interactors;
using Cinnamon.Api.Core.Services.OngoingActivityService.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.OngoingActivityService;

public class CreateOngoingActivityHandler : ICreateOngoingActivityHandler
{
    private readonly IActivityData activityData;
    private readonly IScheduleData scheduleData;
    private readonly IOngoingActivitiesData ongoingActivitiesData;
    private readonly IStudentData studentData;
    private readonly IFamilyMemberData familyMemberData;

    public CreateOngoingActivityHandler(IActivityData activityData,IOngoingActivitiesData ongoingActivitiesData,
        IStudentData studentData, IFamilyMemberData familyMemberData, IScheduleData scheduleData)
    {
        this.activityData = activityData;
        this.ongoingActivitiesData = ongoingActivitiesData;
        this.studentData = studentData;
        this.familyMemberData = familyMemberData;
        this.scheduleData = scheduleData;
    }

    public AppResult<CreateOngoingActivityResult> Execute(CreateOngoingActivityArgs args)
    {
        try
        {
            return ExecuteAsync(args).Result;
        }
        catch (Exception ex)
        {
            return AppResult<CreateOngoingActivityResult>.CreateFailed(ex, "An error occured in CreateOngoingActivityHandler");
        }
    }

    public async Task<AppResult<CreateOngoingActivityResult>> ExecuteAsync(CreateOngoingActivityArgs args)
    {
        try
        {
            if(args.Students == null || args.Students.Count() == 0)
            {
                return AppResult<CreateOngoingActivityResult>.CreateFailed(
                    new ApplicationException("Empty enrolled stundent not allowed"), "Empty enrolled stundent not allowed");
            }

            // check activity and schedule if associated
            var activityRes = await activityData.GetActivityById(args.ActivityId, 
                new Framework.ApiCommand.ApiData.Activity.Request.GetActivityArgs {
                IncludeSchedules = true
            });

            if(!activityRes.Succeeded || activityRes.Result == null || !activityRes.Result.IsSuccess)
            {
                return AppResult<CreateOngoingActivityResult>.CreateFailed(
                    new ApplicationException("Can't find activity id details"), "Can't find activity id details");
            }

            if(!activityRes.Result.Result.Schedules.Any(s => s.Id == args.ScheduleId))
            {
                return AppResult<CreateOngoingActivityResult>.CreateFailed(
                    new ApplicationException("Invalid activity and schedule selected"), "Invalid activity and schedule selected");
            }
            var schedule = activityRes.Result.Result.Schedules.First(s => s.Id == args.ScheduleId);

            var validateStudents = await IsFamilyMembersValid(args.Students, args.CustomerId);
            if(!validateStudents.Succeeded)
            {
                return AppResult<CreateOngoingActivityResult>.CreateFailed(validateStudents.Error.Exception, validateStudents.Message);
            }

            var createOngoingActivityRes = await ongoingActivitiesData.CreateOngoingActivity(
                new Framework.ApiCommand.ApiData.OngoingActivity.Request.CreateOngoingActivityArgs 
            {
                ActivityId = args.ActivityId,
                CustomerId = args.CustomerId,
                PurchaseOrderId = args.PurchaseOrderId,
                ScheduleId = args.ScheduleId
            });

            if(!createOngoingActivityRes.Succeeded || createOngoingActivityRes.Result == null)
            {
                return AppResult<CreateOngoingActivityResult>.CreateFailed(
                    new ApplicationException(createOngoingActivityRes.Message), createOngoingActivityRes.Message);
            }

            if(createOngoingActivityRes.Succeeded && !createOngoingActivityRes.Result.IsSuccess)
            {
                return AppResult<CreateOngoingActivityResult>.CreateFailed(
                    new ApplicationException(createOngoingActivityRes.Result.ErrorInfo?.Message), "An error occured in CreateOngoingActivityHandler");
            }

            //Check if Schedule has Start Expiration Date
            DateTime endExpiration = DateTime.MinValue;
            DateTime startExpiration = DateTime.MinValue; 
            if (schedule.HasExpiration == 1 && schedule.IsSetSession == true)
            {
                startExpiration = schedule.StartDate.Value;
                switch (schedule.SessionName)
                {
                    case "2 Weeks":
                        endExpiration = startExpiration.AddDays(14);
                        break;
                    case "3 Weeks":
                        endExpiration = startExpiration.AddDays(21);
                        break;
                    case "1 Month":
                        endExpiration = startExpiration.AddMonths(1);
                        break;
                    case "2 Months":
                        endExpiration = startExpiration.AddMonths(2);
                        break;
                    default:
                        break;
                }
            }
            //Get Number of Backtracking
            double quotient = (double)schedule.PerUnit2 / 2;
            int numberOfBackTracking = 0;
            if (quotient % 2 == 0)
            {
                numberOfBackTracking = (int)quotient;
            }
            else
            {
                numberOfBackTracking = (int)Math.Ceiling(quotient);
            }
            // enroll the students
            var createStudentRes = await studentData.CreateManyStudent(new Framework.ApiCommand.ApiData.Student.Request.CreateManyStudentArgs {
                CustomerId = args.CustomerId,
                ActivityId = args.ActivityId,
                ScheduleId = schedule.Id,
                NumberOfSessions = schedule.PerUnit2,
                SessionsAttended = 0,
                NumberOfBacktracking = numberOfBackTracking,
                ExpirationEndDate = endExpiration,
                ExpirationStartDate = startExpiration,
                Students = args.Students.Select(s => {
                    return new Framework.ApiCommand.ApiData.Student.Request.CreateManyStudentArgs.StudentDetails {
                        FamilyMemberId = s.FamilyMemberId,
                        Name = s.Name,
                        StudentNo = "00",
                    };
                }),
                OngoingActivityId = createOngoingActivityRes.Result.Result.Id
            });

            if(!createStudentRes.Succeeded || createStudentRes.Result == null || !createStudentRes.Result.IsSuccess)
            {
                return AppResult<CreateOngoingActivityResult>.CreateFailed(
                    new ApplicationException(createStudentRes.Result?.ErrorInfo?.Message), createStudentRes.Message);
            }

            return AppResult<CreateOngoingActivityResult>.CreateSucceeded(new CreateOngoingActivityResult {
                CreatedId = createOngoingActivityRes.Result.Result.Id,
            }, "Successfully created ongoing activity");

        }
        catch (Exception ex)
        {
            return AppResult<CreateOngoingActivityResult>.CreateFailed(ex, "An error occured in CreateOngoingActivityHandler");
        }
    }

    private async Task<AppResult<bool>> IsFamilyMembersValid(IEnumerable<CreateOngoingActivityArgs.Student> students, int customerId)
    {
        try
        {
            var result = await familyMemberData.GetFamilyMemberByCustomerId(customerId);
            if(!result.Succeeded || result.Result == null || !result.Result.IsSuccess)
            {
                return AppResult<bool>.CreateFailed(result.Error.Exception, result.Message);
            }
            var familyMembers = result.Result.Result;

            var familyMembersToDictionary = familyMembers.ToDictionary(f => f.Id);
            foreach(var item in students)
            {
                if(!familyMembersToDictionary.ContainsKey(item.FamilyMemberId))
                {
                    return AppResult<bool>.CreateFailed(new ApplicationException("Unable to determine family member"),"Unable to determine family member");
                }
                item.Name = familyMembersToDictionary[item.FamilyMemberId].Name;
            }
            
            return AppResult<bool>.CreateSucceeded(true, "All students validated.");
        }
        catch (Exception ex)
        {
            return AppResult<bool>.CreateFailed(ex, "An error occured when validating students");
        }
    }
}