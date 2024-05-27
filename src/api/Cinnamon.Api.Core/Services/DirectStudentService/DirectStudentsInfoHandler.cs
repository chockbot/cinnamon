using AutoMapper;
using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Services.AccountService.Handlers;
using Cinnamon.Api.Core.Services.DirectStudentService.Interactors.Results;
using Cinnamon.Api.Core.Services.DirectStudentService.Interactors;
using Cinnamon.Api.Core.Services.DirectStudentService.Handlers;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.DirectStudentService;

public class DirectStudentsInfoHandler : IDirectStudentsInfoHandler
{
    private readonly IGetProfileHandler getProfileHandler;
    private readonly IDirectStudentData directStudentData;
    private readonly IMapper mapper;

    public DirectStudentsInfoHandler(IGetProfileHandler getProfileHandler, 
        IDirectStudentData directStudentData, IMapper mapper)
    {
        this.getProfileHandler = getProfileHandler;
        this.directStudentData = directStudentData;
        this.mapper = mapper;
    }

    public AppResult<DirectStudentsInfoResult> Execute(DirectStudentsInfosArgs args)
    {
        return ExecuteAsync(args).Result;
    }

    public async Task<AppResult<DirectStudentsInfoResult>> ExecuteAsync(DirectStudentsInfosArgs args)
    {
        try
        {
            var providerProfileRes = await getProfileHandler.ExecuteAsync(new AccountService.Interactors.GetProfileArgs {});
            if(!providerProfileRes.Succeeded || providerProfileRes.Result is null)
            {
                return AppResult<DirectStudentsInfoResult>.CreateFailed(new ApplicationException(providerProfileRes.Message), providerProfileRes.Message);
            }

            var directStudentsInfoRes = await directStudentData.StudentInfos(new Framework.ApiCommand.ApiData.DirectStudent.Request.StudentInfosArgs {
                CountPerPage = args.PageCount,
                PageIndex = args.PageIndex,
                ProviderId = providerProfileRes.Result.Id,
                SearchValue = args.SearchValue ?? string.Empty
               
            });
            if(!directStudentsInfoRes.Succeeded || directStudentsInfoRes.Result is null || !directStudentsInfoRes.Result.IsSuccess)
            {
                return AppResult<DirectStudentsInfoResult>.CreateFailed(
                    new ApplicationException(directStudentsInfoRes.Result?.ErrorInfo?.Message), directStudentsInfoRes.Message);
            }

            var result = mapper.Map<IEnumerable<DirectStudentsInfoResult.DirectStudentInfo>>(directStudentsInfoRes.Result.Result);
            foreach (var item in result)
            {
                item.Age = CalculateAge(item.BirthMonth, item.BirthYear);
            }

            return AppResult<DirectStudentsInfoResult>.CreateSucceeded(
                new DirectStudentsInfoResult {DirectStudentInfos = result}, "Successfully get direct students info.");
        }
        catch (Exception ex)
        {
            return AppResult<DirectStudentsInfoResult>.CreateFailed(ex, "An error occured in DirectStudentsInfoHandler.");
        }
    }

    private int CalculateAge(string month, int year)
    {
        int monthInt = month.ToLower() switch {
            "january" => 1,
            "february" => 2,
            "march" => 3,
            "april" => 4,
            "may" => 5,
            "june" => 6,
            "july" => 7,
            "august" => 8,
            "september" => 9,
            "october" => 10,
            "noveber" => 11,
            _ => 12
        };

        DateTime currentDate = DateTime.Today;
        DateTime birthDate = new DateTime(year, monthInt, 1);

        var age = currentDate.Year - birthDate.Year;

        if(birthDate.Month > currentDate.Month) age--;

        return age;
    }
}