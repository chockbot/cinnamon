using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Modules.UploadDriver.Handlers;
using Cinnamon.Api.Core.Providers;
using Cinnamon.Api.Core.Services.AccountService.Handlers;
using Cinnamon.Api.Core.Services.AdminService.Handlers;
using Cinnamon.Api.Core.Services.SeatPlanService.Handler;
using Cinnamon.Api.Core.Services.SeatPlanService.Interactors;
using Cinnamon.Api.Core.Services.SeatPlanService.Interactors.Result;
using Cinnamon.Api.Core.Services.SeatPlanService.Resolver;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.SeatPlanService;

public class CreateSeatPlanTemplateHandler : ICreateSeatPlanTemplateHandler
{
    private readonly ISeatPlanData seatPlanData;
    private readonly FormatterResolver formatterResolver;
    private readonly IGetProfileHandler getProfileHandler;
    private readonly IGetAdminUserByEmailHandler getAdminUserByEmailHandler;
    private readonly IUploadAzureBlob uploadAzureBlob;
    private readonly IJsonSerializationProvider jsonSerializationProvider;

    public CreateSeatPlanTemplateHandler(ISeatPlanData seatPlanData, IGetProfileHandler getProfileHandler,
        IGetAdminUserByEmailHandler getAdminUserByEmailHandler, IUploadAzureBlob uploadAzureBlob,
        IJsonSerializationProvider jsonSerializationProvider)
    {
        this.seatPlanData = seatPlanData;
        this.formatterResolver = new FormatterResolver();
        this.getProfileHandler = getProfileHandler;
        this.getAdminUserByEmailHandler = getAdminUserByEmailHandler;
        this.uploadAzureBlob = uploadAzureBlob;
        this.jsonSerializationProvider = jsonSerializationProvider;
    }
    
    public AppResult<CreateSeatPlanTemplateResult> Execute(CreateSeatPlanTemplateArgs args)
    {
        return ExecuteAsync(args).Result;
    }

    public async Task<AppResult<CreateSeatPlanTemplateResult>> ExecuteAsync(CreateSeatPlanTemplateArgs args)
    {
        try
        {
            var currentLogin = await getProfileHandler.ExecuteAsync(new AccountService.Interactors.GetProfileArgs {});
            if(!currentLogin.Succeeded || currentLogin.Result is null)
            {
                return AppResult<CreateSeatPlanTemplateResult>.CreateFailed(new ApplicationException(currentLogin.Message), currentLogin.Message);
            }
            var profile = currentLogin.Result;

            var adminResult = await getAdminUserByEmailHandler.ExecuteAsync(new AdminService.Interactors.GetAdminUserByEmailArgs { Email = profile.Email });
            if(!adminResult.Succeeded || adminResult.Result is null)
            {
                return AppResult<CreateSeatPlanTemplateResult>.CreateFailed(
                    new ApplicationException("Account is not admin. Invalid request."), "Account is not admin. Invalid request.");
            }

            var getFormatter = await seatPlanData.GetSeatPlanFormatterAsync();
            if(!getFormatter.Succeeded || getFormatter.Result is null || !getFormatter.Result.IsSuccess)
            {
                return AppResult<CreateSeatPlanTemplateResult>.CreateFailed(
                    new ApplicationException("Unable to find selected formatter"), "Unable to find selected formatter");
            }
            var formatter = getFormatter.Result.Result.FirstOrDefault(f => f.Id == args.FormatterId);

            if(formatter is null)
            {
                return AppResult<CreateSeatPlanTemplateResult>.CreateFailed(
                    new ApplicationException("Unable to find selected formatter"), "Unable to find selected formatter");
            }

            var resolveFormatter = (ISeatPlanFormatterHandler)formatterResolver.ResolveFormatter("ISeatPlanFormatterHandler",formatter.Handler);

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(args.ImageFile.FileName);
            var directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "Templates", "Images");
            var filePath = Path.Combine(directoryPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await args.JsonFile.CopyToAsync(stream);
            }

            var generateFormat = await resolveFormatter.ExecuteAsync(new SeatPlanFormatterArgs {
                FileLocation = filePath,
            });
            if(!generateFormat.Succeeded || generateFormat.Result is null)
            {
                return AppResult<CreateSeatPlanTemplateResult>.CreateFailed(
                    new ApplicationException("Unable to generate format"), "Unable to generate format");
            }
            var format = generateFormat.Result.SeatPlanFormat;
            var serializedFormat = jsonSerializationProvider.Serialize(format);

            var uploadResult = await uploadAzureBlob.ExecuteAsync(new Modules.UploadDriver.Interactors.AzureUploadArgs {
                Container = "seat-plan-templates",
                Images = new List<Modules.UploadDriver.Interactors.AzureUploadArgs.Image> {
                    new Modules.UploadDriver.Interactors.AzureUploadArgs.Image {
                        File = args.ImageFile,
                        ImageName = fileName
                    }
                }
            });
            if(!uploadResult.Succeeded || uploadResult.Result is null)
            {
                return AppResult<CreateSeatPlanTemplateResult>.CreateFailed(
                    new ApplicationException("Unable to upload image"), "Unable to upload image");
            }
            var imageUrl = uploadResult.Result.FilePaths.FirstOrDefault();

            var createTemplate = await seatPlanData.CreateSeatPlanTemplateAsync(new Framework.ApiCommand.ApiData.SeatPlan.Request.CreateSeatPlanTemplateArgs {
                Name = args.Name,
                Address = args.Address,
                Enabled = true,
                ImageSrc = imageUrl?.FileSrc ?? string.Empty,
                Payload = serializedFormat,
                SeatPlanFormatterId = args.FormatterId,
                UploadedBy = profile.Id,
                UploadedDate = DateTime.Now
            });

            if(!createTemplate.Succeeded || createTemplate.Result is null)
            {
                return AppResult<CreateSeatPlanTemplateResult>.CreateFailed(
                    new ApplicationException("Unable to create seat plan template"), "Unable to create seat plan template");
            }

            // Delete the JSON file
            File.Delete(filePath);

            return AppResult<CreateSeatPlanTemplateResult>.CreateSucceeded(new CreateSeatPlanTemplateResult {
                Payload = serializedFormat
            }, "Seat plan template created successfully");
        }
        catch (System.Exception error)
        {
            return AppResult<CreateSeatPlanTemplateResult>.CreateFailed(error, "An error occured in CreateSeatPlanTemplateHandler");
        }
    }
}