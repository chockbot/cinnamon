using Cinnamon.Api.Core.Providers;
using Cinnamon.Api.Core.Services.SeatPlanService.Handler;
using Cinnamon.Api.Core.Services.SeatPlanService.Interactors;
using Cinnamon.Api.Core.Services.SeatPlanService.Interactors.Result;
using Cinnamon.Framework.Common;
using SeatPlan = Cinnamon.Framework.Models.SeatPlan;
using PretixSeatPlan = Cinnamon.Framework.Models.SeatPlan.Pretix;

namespace Cinnamon.Api.Core.Services.SeatPlanService;

public class PretixFormatterHandler : ISeatPlanFormatterHandler, IPretixFormatter
{
    private readonly IJsonSerializationProvider jsonSerializationProvider;

    public PretixFormatterHandler(IJsonSerializationProvider jsonSerializationProvider)
    {
        this.jsonSerializationProvider = jsonSerializationProvider;
    }

    public AppResult<SeatPlanFormatterResult> Execute(SeatPlanFormatterArgs args)
    {
        return ExecuteAsync(args).Result;
    }

    public async Task<AppResult<SeatPlanFormatterResult>> ExecuteAsync(SeatPlanFormatterArgs args)
    {
        try
        {
            string json = string.Empty;
            using(StreamReader file = File.OpenText(args.FileLocation))
            {
                json = await file.ReadToEndAsync();
            }

            var pretixFormat = jsonSerializationProvider.Deserialize<PretixFormat>(json);
            if(pretixFormat == null)
            {
                return AppResult<SeatPlanFormatterResult>.CreateFailed(new ApplicationException("Invalid file format"), "Invalid file format");
            }

            IDictionary<string, Row> categoryRowMap = new Dictionary<string, Row>();

            Format format = new();

                        
        }
        catch (System.Exception ex)
        {
            return AppResult<SeatPlanFormatterResult>.CreateFailed(ex, "An error occured in PretixFormatterHandler");
        }
    }
}