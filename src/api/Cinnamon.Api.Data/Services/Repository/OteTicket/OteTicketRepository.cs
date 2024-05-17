using Cinnamon.Api.Data.Repository.Interfaces;
using Cinnamon.Api.Data.Services.Repository.Interfaces;
using Cinnamon.Framework.ApiCommand.ApiData.DTO.OteTicket;
using Cinnamon.Framework.Common;
using Entities = Cinnamon.Api.Data.Repository.Entities;
using System.Linq.Expressions;
using AutoMapper;
using Cinnamon.Framework.ApiCommand.ApiData.DTO.OteSchedule;

namespace Cinnamon.Api.Data.Services.Repository.OteTicket;

public class OteTicketRepository : IOteTicketRepository
{
    private readonly IDataStore dataStore;
    private readonly IMapper mapper;

    public OteTicketRepository(IDataStore dataStore, IMapper mapper)
    {
        this.dataStore = dataStore;
        this.mapper = mapper;
    }
    
    public async Task<AppResult<IEnumerable<OteTicketDTO>>> CreateMany(IEnumerable<OteTicketDTO> tickets, bool includeImageAsResult = false)
    {
        try
        {
            var oteTickets = mapper.Map<IEnumerable<Entities.OteTicket>>(tickets);
            var result = await dataStore.OteTicket.AddRange(oteTickets);
            if(!result.Succeeded || result.Result is null)
            {
                return AppResult<IEnumerable<OteTicketDTO>>.CreateFailed(new ApplicationException(result.Message), result.Message);
            }

            // remove image data to make result body lighter
            if(!includeImageAsResult)
            {
                foreach(var item in result.Result)
                {
                    item.QRImageData = string.Empty;
                }
            }

            var dtoTickets = mapper.Map<IEnumerable<OteTicketDTO>>(result.Result);
            return AppResult<IEnumerable<OteTicketDTO>>.CreateSucceeded(dtoTickets, "Tickets successfully created");
        }
        catch (Exception ex)
        {
            return AppResult<IEnumerable<OteTicketDTO>>.CreateFailed(ex, "An error occured when creating tickets");
        }
    }

    public async Task<AppResult<IEnumerable<OteTicketDTO>>> GetByActivityId(int activityId, int dateId, string searchValue,int searchBy, int? count, int? skip, 
        bool includeCustomer = false, bool includeImageAsResult = false)
    {
        try
        {
            var result = await dataStore.OteTicket.GetByActivityId(activityId, dateId, searchValue,searchBy ,count, skip, includeCustomer, includeImageAsResult);
            if(!result.Succeeded || result.Result is null)
            {
                return AppResult<IEnumerable<OteTicketDTO>>.CreateFailed(new ApplicationException(result.Message), result.Message);
            }

            var dtoTickets = mapper.Map<IEnumerable<OteTicketDTO>>(result.Result);
            return AppResult<IEnumerable<OteTicketDTO>>.CreateSucceeded(dtoTickets, "Successfully get tickets by activity id");
        }
        catch (Exception ex)
        {
            return AppResult<IEnumerable<OteTicketDTO>>.CreateFailed(ex, "An error occured when getting tickets.");
        }
    }

    public async Task<AppResult<OteTicketDTO>> GetByCode(string code)
    {
        try
        {
            var result = await dataStore.OteTicket.FindFirstAsync(t => t.QRCode == code);
            if(!result.Succeeded || result.Result is null)
            {
                return AppResult<OteTicketDTO>.CreateFailed(new ApplicationException(result.Message), result.Message);
            }

            var dtoTicket = mapper.Map<OteTicketDTO>(result.Result);
            return AppResult<OteTicketDTO>.CreateSucceeded(dtoTicket, "Successfullt get ticket by code.");
        }
        catch (Exception ex)
        {
            return AppResult<OteTicketDTO>.CreateFailed(ex, "An error occured when getting ticket by code");
        }
    }

    public async Task<AppResult<OteTicketDTO>> Update(OteTicketDTO ticket)
    {
        try
        {
            var ticketRes = await dataStore.OteTicket.FindFirstAsync(t => t.Id == ticket.Id);
            if (!ticketRes.Succeeded || ticketRes.Result is null)
            {
                return AppResult<OteTicketDTO>.CreateFailed(new ApplicationException(ticketRes.Message), ticketRes.Message);
            }
            ticketRes.Result.Status = ticket.Status;

            var updatedRes = await dataStore.OteTicket.Update(ticketRes.Result);
            if (!updatedRes.Succeeded || updatedRes.Result is null)
            {
                return AppResult<OteTicketDTO>.CreateFailed(new ApplicationException(updatedRes.Message), updatedRes.Message);
            }

            var dtoTicket = mapper.Map<OteTicketDTO>(updatedRes.Result);
            return AppResult<OteTicketDTO>.CreateSucceeded(dtoTicket, "Ticket successfully updated");
        }
        catch (Exception ex)
        {
            return AppResult<OteTicketDTO>.CreateFailed(ex, "An error occured when updating ticket");
        }
    }
    public async Task<AppResult<IEnumerable<OteScheduleDTO>>> GetTicketDetails(int activityId, int oteDateId)
    {
        try
        {
            var result = await dataStore.OteTicket.GetTicketDetails(activityId, oteDateId);
            if (!result.Succeeded || result.Result == null)
            {
                return AppResult<IEnumerable<OteScheduleDTO>>.CreateFailed(result.Error.Exception, result.Message);
            }
            var ticket = result.Result.Select(s =>
            {
                var ticketDTO = new OteScheduleDTO
                {
                    ActivityId = activityId,
                    From = s.From,
                    To = s.To,
                    Recurrences = s.Recurrences,
                    OteSchedulePricingDTO = new OteSchedulePricingDTO
                    {
                        Name = s.OteSchedulePricingDTO.Name,
                        Description = s.OteSchedulePricingDTO.Description,
                        MaxSlots = s.OteSchedulePricingDTO.MaxSlots,
                        Sold = s.OteSchedulePricingDTO.Sold,
                        Available = s.OteSchedulePricingDTO.MaxSlots - s.OteSchedulePricingDTO.Sold
                    }
                };
                return ticketDTO;
            });
            return AppResult<IEnumerable<OteScheduleDTO>>.CreateSucceeded(ticket, "Successfully get ticket details");
        }
        catch (Exception ex)
        {
            return AppResult<IEnumerable<OteScheduleDTO>>.CreateFailed(ex, "An error occured when getting ticket details");
        }
    }

    public async Task<AppResult<IEnumerable<OteTicketDTO>>> GetByPurchaseOrderId(int purchaseOrderId, bool includeCustomer = false, bool includeImageAsResult = false)
    {
        try
        {
            var result = await dataStore.OteTicket.GetByPurchaseOrderId(purchaseOrderId, includeCustomer, includeImageAsResult);
            if(!result.Succeeded || result.Result is null)
            {
                return AppResult<IEnumerable<OteTicketDTO>>.CreateFailed(new ApplicationException(result.Message), result.Message);
            }

            var dtoTickets = mapper.Map<IEnumerable<OteTicketDTO>>(result.Result);
            return AppResult<IEnumerable<OteTicketDTO>>.CreateSucceeded(dtoTickets, "Successfully get tickets by purchase order id");
        }
        catch (Exception ex)
        {
            return AppResult<IEnumerable<OteTicketDTO>>.CreateFailed(ex, "An error occured when getting tickets.");
        }
    }

    public async Task<AppResult<OteSharedLinkDTO>> CreateSharedLink(OteSharedLinkDTO sharedLinkdto)
    {
        try
        {
            var entity = mapper.Map<Entities.OteSharedLink>(sharedLinkdto);

            var result = await dataStore.OteSharedLink.Add(entity);
            if(!result.Succeeded || result.Result is null)
            {
                return AppResult<OteSharedLinkDTO>.CreateFailed(new ApplicationException(result.Message), result.Message);
            }

            var dto = mapper.Map<OteSharedLinkDTO>(result.Result);
            return AppResult<OteSharedLinkDTO>.CreateSucceeded(dto, "Ote shared link successfully created.");
        }
        catch (Exception ex)
        {
            return AppResult<OteSharedLinkDTO>.CreateFailed(ex, "An error occured when creating shared link.");
        }
    }

    public async Task<AppResult<OteSharedLinkDTO>> GetSharedLinks(string token, string guid)
    {
        try
        {
            var result = await dataStore.OteSharedLink.FindFirstAsync(o => o.Token == token && o.Guid == guid);
            if(!result.Succeeded || result.Result is null)
            {
                return AppResult<OteSharedLinkDTO>.CreateFailed(new ApplicationException(result.Message), result.Message);
            }

            var dto = mapper.Map<OteSharedLinkDTO>(result.Result);
            return AppResult<OteSharedLinkDTO>.CreateSucceeded(dto, "Successfully get shared link using token and guid.");
        }
        catch (Exception ex)
        {
            return AppResult<OteSharedLinkDTO>.CreateFailed(ex, "An error occured when getting shared link.");
        }
    }

    public async Task<AppResult<IEnumerable<OteSharedLinkDTO>>> GetSharedLinks(int activityId, int dateId)
    {
        try
        {
            var result = await dataStore.OteSharedLink.FindAsync(o => o.ActivityId == activityId && o.OteDateId == dateId);
            if(!result.Succeeded || result.Result is null)
            {
                return AppResult<IEnumerable<OteSharedLinkDTO>>.CreateFailed(new ApplicationException(result.Message), result.Message);
            }

            var dtos = mapper.Map<IEnumerable<OteSharedLinkDTO>>(result.Result);
            return AppResult<IEnumerable<OteSharedLinkDTO>>.CreateSucceeded(dtos, "Successfully get shared links by activity id and date id.");
        }
        catch (Exception ex)
        {
            return AppResult<IEnumerable<OteSharedLinkDTO>>.CreateFailed(ex, "An error occured when getting shared links.");
        }
    }

    public async Task<AppResult<OteSharedLinkDTO>> UpdateSharedLinkStatus(int id, bool status)
    {
        try
        {
            var sharedLinkRes = await dataStore.OteSharedLink.FindFirstAsync(s => s.Id == id);
            if(!sharedLinkRes.Succeeded || sharedLinkRes.Result is null)
            {
                return AppResult<OteSharedLinkDTO>.CreateFailed(
                    new ApplicationException("Unable to find shared link to update."), "Unable to find shared link to update.");
            }
            var sharedLink = sharedLinkRes.Result;
            
            sharedLink.Enable = status;
            var result = await dataStore.OteSharedLink.Update(sharedLink);
            if(!result.Succeeded || result.Result is null)
            {
                return AppResult<OteSharedLinkDTO>.CreateFailed(new ApplicationException(result.Message), result.Message);
            }

            var dtos = mapper.Map<OteSharedLinkDTO>(result.Result);
            return AppResult<OteSharedLinkDTO>.CreateSucceeded(dtos, "Successfully update shared link status.");
        }
        catch (Exception ex)
        {
            return AppResult<OteSharedLinkDTO>.CreateFailed(ex, "An error occured when updating shared link status.");
        }
    }

    public async Task<AppResult<int>> CountBookedTickets(int activityId)
    {
        try
        {
            Expression<Func<Entities.OteTicket, bool>> filter = 
                a => a.ActivityId == activityId;

            var result = await dataStore.OteTicket.Count(filter);

            return AppResult<int>.CreateSucceeded(result.Result, "Successfully count booked tickets.");
        }
        catch (Exception ex)
        {
            return AppResult<int>.CreateFailed(ex, "An error occured when counting the booked tickets.");
        }
    }
}