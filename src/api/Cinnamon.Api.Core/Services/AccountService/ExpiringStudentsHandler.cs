using Cinnamon.Api.Core.Config;
using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Modules.NotificationDriver.Handler;
using Cinnamon.Api.Core.Services.AccountService.Handlers;
using Cinnamon.Api.Core.Services.AccountService.Interactors;
using Cinnamon.Api.Core.Services.AccountService.Interactors.Results;
using Cinnamon.Framework.Common;
using Flurl;

namespace Cinnamon.Api.Core.Services.AccountService;

public class ExpiringStudentsHandler : IExpiringStudentsHandler
{
    private readonly IStudentData studentData;
    private readonly IExpiringStudentNotificationHandler expiringStudentNotificationHandler;
    private readonly ApplicationConfig applicationConfig;

    public ExpiringStudentsHandler(IStudentData studentData, IExpiringStudentNotificationHandler expiringStudentNotificationHandler,
        ApplicationConfig applicationConfig)
    {
        this.studentData = studentData;
        this.expiringStudentNotificationHandler = expiringStudentNotificationHandler;
        this.applicationConfig = applicationConfig;
    }

    public AppResult<ExpiringStudentsResult> Execute(ExpiringStudentsArgs args)
    {
        try
        {
            return ExecuteAsync(args).Result;
        }
        catch (Exception ex)
        {
            return AppResult<ExpiringStudentsResult>.CreateFailed(ex, "An error occured in IExpiringStudentsHandler");
        }
    }

    public async Task<AppResult<ExpiringStudentsResult>> ExecuteAsync(ExpiringStudentsArgs args)
    {
        try
        {
            var studentsRes = await studentData.GetExpiringStudents();

            if(!studentsRes.Succeeded || studentsRes.Result == null || !studentsRes.Result.IsSuccess)
            {
                return AppResult<ExpiringStudentsResult>.CreateFailed(new ApplicationException(studentsRes.Message), studentsRes.Message);
            }

            var now = DateTime.Now;
            var uniqueStudents = studentsRes.Result.Result.Distinct().ToList();

            var tasks = uniqueStudents.Select(s =>
            {
                var link = applicationConfig.FrontendUrl.AppendPathSegment($"explore/{s.Handler}");
                return expiringStudentNotificationHandler.ExecuteAsync(new Modules.NotificationDriver.Interactors.ExpiringStudentNotificationArgs
                {
                    ActivityLink = link,
                    ActivityTitle = s.Title,
                    Amount = s.Price,
                    DateSend = now,
                    Email = s.Email,
                    ExpiredDate = s.DateEnd,
                    FirstName = s.FirstName,
                    LastName = s.LastName,
                    Address = s.Address,
                    APrice = s.APrice,
                    Count = s.Count,
                    ImageLocation = s.ImageLocation,
                    Rating = s.Rating
                });
            });

            await Task.WhenAll(tasks);

            return AppResult<ExpiringStudentsResult>.CreateSucceeded(new ExpiringStudentsResult(), "Successfully notified expiring students");
        }
        catch (Exception ex)
        {
            return AppResult<ExpiringStudentsResult>.CreateFailed(ex, "An error occured in IExpiringStudentsHandler");
        }
    }
}