using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Services.ActivityService.Handlers;
using Cinnamon.Api.Core.Services.ActivityService.Interactors;
using Cinnamon.Api.Core.Services.ActivityService.Interactors.Results;
using Cinnamon.Framework.Common;
using Ganss.XSS;
using System.Security.Claims;

namespace Cinnamon.Api.Core.Services.ActivityService;

public class CreateActivityHandler : ICreateActivityHandler
{
    private readonly IHttpContextAccessor httpContext;
    private readonly IActivityData activityData;
    private readonly IScheduleData scheduleData;
    private readonly ICustomerData customerData;
    private readonly HtmlSanitizer htmlSanitizer;
    private readonly IGenerateActivityHandler generateActivityHandler;
    private readonly IAddOnsData addOnsData;

    public CreateActivityHandler(IHttpContextAccessor httpContext, IActivityData activityData, 
        IScheduleData scheduleData, ICustomerData customerData, IGenerateActivityHandler generateActivityHandler, IAddOnsData addOnsData)
    {
        this.httpContext = httpContext;
        this.activityData = activityData;
        this.scheduleData = scheduleData;
        this.customerData = customerData;
        this.generateActivityHandler = generateActivityHandler;
        this.addOnsData = addOnsData;

        this.htmlSanitizer = new 
            HtmlSanitizer(
                allowedTags: new string[] {"p","strong", "em", "ul", "ol", "li", "br"});
    }

    public AppResult<CreateActivityResult> Execute(CreateActivityArgs args)
    {
        try
        {
            return ExecuteAsync(args).Result;
        }
        catch (Exception ex)
        {
            return AppResult<CreateActivityResult>.CreateFailed(ex, "An error occurred in CreateActivityHandler");
        }
    }

    public async Task<AppResult<CreateActivityResult>> ExecuteAsync(CreateActivityArgs args)
    {
        try
        {
            // get customer id saved in claims
            var customerId = httpContext.HttpContext?.User.FindFirstValue("UserId");
            if(customerId == null)
            {
                return AppResult<CreateActivityResult>.CreateFailed(
                    new ApplicationException("Unable to determine current account login"), "Unable to determine current account login");
            }
            int id = Convert.ToInt32(customerId);

            const int maxWords = 80;
            var customerBringLength = WordsLenght(args.CustomerBringWithThem ?? string.Empty);
            var specificProvideLength = WordsLenght(args.SpecificsYouWillProvide ?? string.Empty);
            var classPoliciesLength = WordsLenght(args.ClassPolicies ?? string.Empty);

            if (customerBringLength > maxWords || specificProvideLength > maxWords || classPoliciesLength > maxWords)
            {
                return AppResult<CreateActivityResult>.CreateFailed(
                    new ApplicationException($"Limit only of {maxWords} for Customer Bring/Specific Provide/Class Policies fields."), 
                        $"Limit only of {maxWords} for Customer Bring/Specific Provide/Class Policies fields.");
            }

            // generate activity handler
            var generateHandlerRes = await generateActivityHandler.ExecuteAsync(new GenerateActivityHandlerArgs {
                ActivityName = args.Title
            });
            if(!generateHandlerRes.Succeeded || generateHandlerRes.Result == null)
            {
                return AppResult<CreateActivityResult>.CreateFailed(new ApplicationException(generateHandlerRes.Message), generateHandlerRes.Message);
            }
            var handlerName = generateHandlerRes.Result.GeneratedHandler;

            var activityRes = await activityData.CreateActivity(new Framework.ApiCommand.ApiData.Activity.Request.CreateActivityArgs {
                ActivityLevel           = args.ActivityLevel,
                AdditionalRequirements  = args.AdditionalRequirements,
                Address1                = args.Address1,
                Address2                = args.Address2,
                CanAdultsJoin           = args.CanAdultsJoin,
                City                    = args.City,
                Subdivision             = args.Subdivision,
                Region                  = args.Region,
                Barangay                = args.Barangay,
                PostalCode              = args.PostalCode,
                CustomerBringWithThem   = customerBringLength == 0 ? string.Empty : htmlSanitizer.Sanitize(args.CustomerBringWithThem ?? string.Empty),
                CustomerId              = id,
                Description             = htmlSanitizer.Sanitize(args.Description),
                District                = args.District,
                ExperienceCategoryId    = args.ExperienceCategoryId,
                ExperienceTypeId        = args.ExperienceTypeId,
                IsPublished             = args.IsPublished,
                MinimumAge              = args.MinimumAge,
                Price                   = args.Price,
                Remarks                 = args.Remarks,
                ScheduleIndicator       = args.ScheduleIndicator,
                Searchtag1              = args.SearchTags.Count() >= 1 ? args.SearchTags.ElementAt(0) : null,
                Searhtag2               = args.SearchTags.Count() >= 2 ? args.SearchTags.ElementAt(1) : null,
                Searhtag3               = args.SearchTags.Count() >= 3 ? args.SearchTags.ElementAt(2) : null,
                Searchtag4              = args.SearchTags.Count() >= 4 ? args.SearchTags.ElementAt(3) : null,
                Searchtag5              = args.SearchTags.Count() >= 5 ? args.SearchTags.ElementAt(4) : null,
                SkillLevel              = args.SkillLevel,
                SpecificsYouWillProvide = specificProvideLength == 0 ? string.Empty : htmlSanitizer.Sanitize(args.SpecificsYouWillProvide ?? string.Empty),
                SubCategoryId           = args.SubCategoryId,
                Title                   = args.Title,
                Handler                 = handlerName,
                PinnedLocation          = args.PinnedLocation,
                Status                  = args.Status,
                ExperienceCreationType  = args.ExperienceCreationType,
                ClassPolicies           = classPoliciesLength == 0 ? string.Empty : htmlSanitizer.Sanitize(args.ClassPolicies ?? string.Empty),
                VideoLink               = args.VideoLink,
            });

            if(!activityRes.Succeeded || activityRes.Result == null)
            {
                return AppResult<CreateActivityResult>.CreateFailed(new ApplicationException(activityRes.Message), activityRes.Message);
            }

            if(activityRes.Succeeded && !activityRes.Result.IsSuccess)
            {
                return AppResult<CreateActivityResult>.CreateFailed(new ApplicationException(activityRes.Result.ErrorInfo?.Message), "An error occurred in CreateActivityHandler");
            }
            var activity = activityRes.Result.Result;

            // create activity schedules
            int order = 0;
            var createdSchedules = await scheduleData.CreateManySchedules(new Framework.ApiCommand.ApiData.Schedule.Request.CreateManySchedulesArgs {
                ActivityId = activity.Id,
                Schedules = args.ActivitySchedules is not null ? args.ActivitySchedules.OrderBy(s => s.Order).Select(s => {
                    order += 1;
                    return new Framework.ApiCommand.ApiData.Schedule.Request.CreateManySchedulesArgs.Schedule {
                        DateTime         = s.DateTime,
                        Name             = s.Name,
                        PerUnit1         = s.PerUnit1,
                        PerUnit2         = s.PerUnit2,
                        Price            = s.Price,
                        PriceUnit1       = s.PriceUnit1,
                        PriceUnit2       = s.PriceUnit2,
                        UnitPrice        = s.UnitPrice,
                        Order            = order,
                        IsActiveSchedule = s.IsActiveSchedule,
                        IsSetSession     = s.IsSetSession,
                        SessionName      = s.SessionName,
                        HasExpiration    = s.HasExpiration,
                        StartDate        = s.StartDate,
                        ScheduleType     = s.ScheduleType,
                        PriceType        = s.PriceType,
                        SchedulingUrl    = s.SchedulingUrl,
                        ActivityScheduleTimes = s.ActivityScheduleTimes is not null ? s.ActivityScheduleTimes.Select(s => new Framework.ApiCommand.ApiData.Schedule.Request.CreateManySchedulesArgs.ActivityScheduleTime
                        {
                            DayOfWeek = s.DayOfWeek,
                            EndTime = s.EndTime,
                            StartTime = s.StartTime,
                            IsEnabled = s.IsEnabled
                        }) : Enumerable.Empty<Framework.ApiCommand.ApiData.Schedule.Request.CreateManySchedulesArgs.ActivityScheduleTime>()
                    };
                }) : Enumerable.Empty<Cinnamon.Framework.ApiCommand.ApiData.Schedule.Request.CreateManySchedulesArgs.Schedule>()
            });

            if(!createdSchedules.Succeeded || createdSchedules.Result == null)
            {
                return AppResult<CreateActivityResult>.CreateFailed(new ApplicationException(createdSchedules.Message), createdSchedules.Message);
            }

            if(createdSchedules.Succeeded && !createdSchedules.Result.IsSuccess)
            {
                return AppResult<CreateActivityResult>.CreateFailed(new ApplicationException(createdSchedules.Result.ErrorInfo?.Message), "An error occurred in CreateActivityHandler");
            }

            //create add-ons
            if (args.AddOns.Count() != 0)
            {
                int addOnsOrder = 0;
                var createdAddOns = await addOnsData.CreateManyAddOns(new Framework.ApiCommand.ApiData.AddOns.Request.CreateAddOnsArgs
                {
                    ActivityId = activity.Id,
                    AddOns = args.AddOns is not null ? args.AddOns.OrderBy(a => a.Order).Select(s =>
                    {
                        addOnsOrder += 1;
                        return new Framework.ApiCommand.ApiData.AddOns.Request.CreateAddOnsArgs.AddOn
                        {
                            Name        = s.Name,
                            Price       = s.Price,
                            UnitPrice   = s.UnitPrice,
                            Description = s.Description,
                            Order       = addOnsOrder
                        };
                    }) : Enumerable.Empty<Cinnamon.Framework.ApiCommand.ApiData.AddOns.Request.CreateAddOnsArgs.AddOn>()
                });

                if (!createdAddOns.Succeeded || createdAddOns.Result == null)
                {
                    return AppResult<CreateActivityResult>.CreateFailed(new ApplicationException(createdAddOns.Message), createdAddOns.Message);
                }

                if (createdAddOns.Succeeded && !createdAddOns.Result.IsSuccess)
                {
                    return AppResult<CreateActivityResult>.CreateFailed(new ApplicationException(createdSchedules.Result.ErrorInfo?.Message), "An error occurred in CreateActivityHandler");
                }
            }

            // update customer to maker status
            var customer = await customerData.GetCustomerById(id);
            if(!customer.Succeeded || customer.Result == null)
            {
                return AppResult<CreateActivityResult>.CreateFailed(new ApplicationException(customer.Message), customer.Message);
            }

            if(customer.Succeeded && !customer.Result.IsSuccess)
            {
                return AppResult<CreateActivityResult>.CreateFailed(new ApplicationException(customer.Result.ErrorInfo?.Message), "An error occurred in CreateActivityHandler");
            }

            if(!customer.Result.Result.IsMaker)
            {
                var updatedCustomer = await customerData.UpdateCustomer(new Framework.ApiCommand.ApiData.Customer.Request.UpdateCustomerArgs {
                    IsMaker = true,
                    CustomerId = id
                });
                
                if((!updatedCustomer.Succeeded || updatedCustomer.Result == null) || (updatedCustomer.Succeeded && !updatedCustomer.Result.IsSuccess))
                {
                    return AppResult<CreateActivityResult>.CreateFailed(
                        new ApplicationException("An error occured when trying to update customer to maker"), "An error occurred when trying to update customer to maker");
                }
            }

            return AppResult<CreateActivityResult>.CreateSucceeded(new CreateActivityResult {
                ActivityId              = activity.Id,
                ActivityLevel           = activity.ActivityLevel,
                AdditionalRequirements  = activity.AdditionalRequirements,
                Address1                = activity.Address1,
                Address2                = activity.Address2,
                CanAdultsJoin           = activity.CanAdultsJoin,
                City                    = activity.City,
                Subdivision             = activity.Subdivision,
                Barangay                = activity.Barangay,
                PostalCode              = activity.PostalCode,    
                Region                  = activity.Region,  
                CustomerBringWithThem   = activity.CustomerBringWithThem,
                Description             = activity.Description,
                District                = activity.District,
                ExperienceCategoryId    = activity.ExperienceCategoryId,
                ExperienceTypeId        = args.ExperienceTypeId,
                IsPublished             = activity.IsPublished,
                MinimumAge              = activity.MinimumAge,
                Price                   = activity.Price,
                Remarks                 = activity.Remarks,
                SearchTags              = args.SearchTags,
                SkillLevel              = activity.SkillLevel,
                SpecificsYouWillProvide = activity.SpecificsYouWillProvide,
                SubCategoryId           = activity.SubCategoryId,
                Title                   = activity.Title,
                Handler                 = activity.Handler,
                ClassPlicies            = activity.ClassPolicies,
                VideoLink               = activity.VideoLink,
                ActivitySchedules = createdSchedules.Result.Result.Select(s => {
                    return new CreateActivityResult.ActivitySchedule {
                        DateTime         = s.DateTime,
                        Name             = s.Name,
                        PerUnit1         = s.PerUnit1,
                        PerUnit2         = s.PerUnit2,
                        Price            = s.Price,
                        PriceUnit1       = s.PriceUnit1,
                        PriceUnit2       = s.PriceUnit2,
                        UnitPrice        = s.UnitPrice,
                        Order            = order,
                        IsActiveSchedule = s.IsActiveSchedule,
                        IsSetSession     = s.IsSetSession,
                        SessionName      =s.SessionName,
                        HasExpiration    = s.HasExpiration,
                        StartDate        = s.StartDate
                    };
                })

            }, "Successfully creating activity");
        }
        catch (Exception ex)
        {
            return AppResult<CreateActivityResult>.CreateFailed(ex, "An error occurred in CreateActivityHandler");
        }
    }

    private int WordsLenght(string text)
    {
        var words = htmlSanitizer
                        .Sanitize(text)
                        .Replace("<br>"," ").Replace("</br>"," ")
                        .Replace("<p>","").Replace("</p>","")
                        .Replace("<strong>","").Replace("</strong>","")
                        .Replace("<em>","").Replace("</em>","")
                        .Replace("<ul>","").Replace("</ul>","")
                        .Replace("<ol>","").Replace("</ol>","")
                        .Replace("<li>","").Replace("</li>","")
                        .Trim()
                        .Split(" ")
                        .Where(t => !string.IsNullOrEmpty(t.Trim()));
        
        return words.Count();
    }
}