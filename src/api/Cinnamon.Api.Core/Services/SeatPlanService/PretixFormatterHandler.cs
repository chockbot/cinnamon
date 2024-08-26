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

            var pretixFormat = jsonSerializationProvider.Deserialize<PretixSeatPlan.PretixFormat>(json);
            if(pretixFormat == null)
            {
                return AppResult<SeatPlanFormatterResult>.CreateFailed(new ApplicationException("Invalid file format"), "Invalid file format");
            }

            var categoryRowMap = GenerateCategoryRowMap(pretixFormat);
            var categories = GenerateSeatMap(categoryRowMap);

            SeatPlan.Format format = new SeatPlan.Format{
                Name = pretixFormat.name,
                Categories = new Dictionary<string, SeatPlan.Category>()
            };

            foreach (var category in categories)
            {
                format.Categories.Add(category.Value.Uuid, category.Value);
            }

            return AppResult<SeatPlanFormatterResult>.CreateSucceeded(new SeatPlanFormatterResult {SeatPlanFormat = format}, "Seat Plan formatted successfully");
        }
        catch (System.Exception ex)
        {
            return AppResult<SeatPlanFormatterResult>.CreateFailed(ex, "An error occured in PretixFormatterHandler");
        }
    }

    private IDictionary<string, SeatPlan.Row> GenerateCategoryRowMap(PretixSeatPlan.PretixFormat pretixFormat)
    {
        IDictionary<string, SeatPlan.Row> categoryRowMap = new Dictionary<string, SeatPlan.Row>();

        for (int i = 0; i < pretixFormat.zones.Count; i++)
        {
            var zone = pretixFormat.zones[i];
            for (int x = 0; x < zone.rows.Count; x++)
            {
                var row = zone.rows[x];
                string rowNumber = row.row_number;

                for (int s = 0; s < row.seats.Count; s++)
                {
                    var seat = row.seats[s];
                    var categoryRowKey = $"{seat.category}-{rowNumber}";

                    if (!categoryRowMap.ContainsKey(categoryRowKey))
                    {
                        var cat = pretixFormat.categories.FirstOrDefault(c => c.name == seat.category);
                        string color = cat?.color ?? string.Empty;
                        string uuid = Guid.NewGuid().ToString();

                        SeatPlan.Row categoryRow = new()
                        {
                            Uuid = uuid,
                            Color = color,
                            Category = seat.category,
                            RowNumber = rowNumber,
                            Seats = new Dictionary<string, SeatPlan.Seat>()
                        };
                        categoryRow.Seats.Add(seat.uuid, new SeatPlan.Seat
                        {
                            SeatNumber = seat.seat_number,
                            Uuid = seat.uuid
                        });
                        categoryRowMap.Add(categoryRowKey, categoryRow);
                    }
                    else
                    {
                        categoryRowMap[categoryRowKey].Seats.Add(seat.uuid, new SeatPlan.Seat
                        {
                            SeatNumber = seat.seat_number,
                            Uuid = seat.uuid
                        });
                    }
                }
            }
        }

        return categoryRowMap;
    }

    private IDictionary<string, SeatPlan.Category> GenerateSeatMap(IDictionary<string, SeatPlan.Row> categoryRowMap)
    {
        IDictionary<string, SeatPlan.Category> seatMap = new Dictionary<string, SeatPlan.Category>();
        foreach (var categoryRow in categoryRowMap)
        {
            var category = categoryRow.Value.Category;
            string rowUuid = Guid.NewGuid().ToString();

            if (!seatMap.ContainsKey(category))
            {
                string categoryUuid = Guid.NewGuid().ToString();
                SeatPlan.Category seatCategory = new()
                {
                    Uuid = categoryUuid,
                    Color = categoryRow.Value.Color,
                    Name = category,
                    Rows = new Dictionary<string, SeatPlan.Row>()
                };
                seatCategory.Rows.Add(rowUuid, new SeatPlan.Row
                {
                    Uuid = rowUuid,
                    RowNumber = categoryRow.Value.RowNumber,
                    Seats = categoryRow.Value.Seats
                });

                seatMap.Add(category, seatCategory);
            }
            else
            {
                if(seatMap[category].Rows.ContainsKey(rowUuid))
                {
                    foreach (var seat in categoryRow.Value.Seats)
                    {
                        seatMap[category].Rows[rowUuid].Seats.Add(seat.Key, seat.Value);
                    }
                }
                else 
                {
                    seatMap[category].Rows.Add(rowUuid, new SeatPlan.Row
                    {
                        Uuid = rowUuid,
                        RowNumber = categoryRow.Value.RowNumber,
                        Seats = categoryRow.Value.Seats
                    });
                }
            }
        }
        return seatMap;
    }
}