using AutoMapper;
using Cinnamon.Api.Data.Repository.Interfaces;
using Cinnamon.Framework.ApiCommand.ApiData.DTO.DynamicContent;
using Cinnamon.Framework.Common;
using Entities = Cinnamon.Api.Data.Repository.Entities;
using Cinnamon.Api.Data.Extensions;
using Cinnamon.Api.Data.Services.Repository.Interfaces;

namespace Cinnamon.Api.Data.Services.Repository.DynamicContent;

public class DynamicContentRepository : IDynamicContnetRepository
{
    private readonly IDataStore dataStore;
    private readonly IMapper mapper;

    public DynamicContentRepository(IDataStore dataStore, IMapper mapper)
    {
        this.dataStore = dataStore;
        this.mapper = mapper;
    }

    public async Task<AppResult<DynamicContentDTO>> CreateDynamicContent(DynamicContentDTO args)
    {
        try
        {
            var entity = mapper.Map<Entities.DynamicContent>(args);

            var result = await dataStore.DynamicContent.Add(entity);
            if(!result.Succeeded || result.Result is null)
            {
                return AppResult<DynamicContentDTO>.CreateFailed(new ApplicationException(result.Message), result.Message);
            }

            var dto = mapper.Map<DynamicContentDTO>(result.Result);
            return AppResult<DynamicContentDTO>.CreateSucceeded(dto, "Successfully create dynamic content.");
        }
        catch (Exception ex)
        {
            return AppResult<DynamicContentDTO>.CreateFailed(ex, "An error occured when creating dynamic content.");
        }
    }

    public async Task<AppResult<DynamicContentDTO>> UpdateDynamicContent(DynamicContentDTO args)
    {
        try
        {
            var dynamicContentRes = await dataStore.DynamicContent.FindFirstAsync(d => d.Id == args.Id);
            if(!dynamicContentRes.Succeeded || dynamicContentRes.Result is null)
            {
                return AppResult<DynamicContentDTO>.CreateFailed(new ApplicationException(dynamicContentRes.Message), dynamicContentRes.Message);
            }
            var dynamicContent = dynamicContentRes.Result;

            dynamicContent.Identifier = args.Identifier ?? dynamicContent.Identifier;
            dynamicContent.Title = args.Title ?? dynamicContent.Title;
            dynamicContent.Description = args.Description ?? dynamicContent.Description;
            dynamicContent.Content = args.Content ?? dynamicContent.Content;
            dynamicContent.DateLastUpdated = args.DateLastUpdated.SetKindUtc();

            var result = await dataStore.DynamicContent.Update(dynamicContent);
            if(!result.Succeeded || result.Result is null)
            {
                return AppResult<DynamicContentDTO>.CreateFailed(new ApplicationException(result.Message), result.Message);
            }

            var dto = mapper.Map<DynamicContentDTO>(result.Result);
            return AppResult<DynamicContentDTO>.CreateSucceeded(dto, "Dynamic content successfully updated.");
        }
        catch (Exception ex)
        {
            return AppResult<DynamicContentDTO>.CreateFailed(ex, "An error occured when updating dynamic content.");
        }
    }

    public async Task<AppResult<DynamicContentDTO>> GetDynamicContent(string identifier)
    {
        try
        {
            var result = await dataStore.DynamicContent.FindFirstAsync(d => d.Identifier == identifier);
            if(!result.Succeeded || result.Result is null)
            {
                return AppResult<DynamicContentDTO>.CreateFailed(new ApplicationException(result.Message), result.Message);
            }

            var dto = mapper.Map<DynamicContentDTO>(result.Result);
            return AppResult<DynamicContentDTO>.CreateSucceeded(dto, "Successfully get dynamic content dto.");
        }
        catch (Exception ex)
        {
            return AppResult<DynamicContentDTO>.CreateFailed(ex, "An error occured when getting dynamic content.");
        }
    }

}