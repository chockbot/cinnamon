using AutoMapper;
using Cinnamon.Api.Data.Services.Repository.Interfaces;
using Cinnamon.Framework.ApiCommand.ApiData;
using Cinnamon.Framework.ApiCommand.ApiData.Disbursement.Request;
using Cinnamon.Framework.ApiCommand.ApiData.Disbursement.Response;
using Microsoft.AspNetCore.Mvc;
using Cinnamon.Framework.ApiCommand.ApiData.DTO.Disbursement;

namespace Cinnamon.Api.Data.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DisbursementController : ControllerBase 
{
    private readonly IDisbursementRepository disbursementRepository;
    private readonly IMapper mapper;

    public DisbursementController(IDisbursementRepository disbursementRepository, IMapper mapper)
    {
        this.disbursementRepository = disbursementRepository;
        this.mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(typeof(GetDisbursementsResult), StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Index([FromQuery] GetDisbursementsArgs args)
    {
        try
        {
            var result = await disbursementRepository.GetDisbursements(args.Status, args.Count ?? 0, args.Skip ?? 0);
            if(!result.Succeeded || result.Result is null)
            {
                return new JsonResult(new GetDisbursementsResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }
            
            return new JsonResult(new GetDisbursementsResult { IsSuccess = true, Result = result.Result });
        }
        catch (Exception ex)
        {
            return new JsonResult(new GetDisbursementsResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [HttpPost]
    [ProducesResponseType(typeof(CreateDisbursementResult), StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Index([FromBody] CreateDisbursementArgs args)
    {
        try
        {
            var dtos = mapper.Map<IEnumerable<DisbursementDTO>>(args.Disbursements);
            var studentIds = args.StudentIds ?? Enumerable.Empty<int>();
            var purchaseOrderIds = args.PurchaseOrderIds ?? Enumerable.Empty<int>();

            var result = await disbursementRepository.CreateDisbursements(dtos, studentIds, purchaseOrderIds);
            if(!result.Succeeded || result.Result is null)
            {
                return new JsonResult(new CreateDisbursementResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new CreateDisbursementResult { IsSuccess = true, Result = result.Result });
        }
        catch (Exception ex)
        {
            return new JsonResult(new CreateDisbursementResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("CreateDisbursementBulk")]
    [HttpPost]
    [ProducesResponseType(typeof(CreateDisbursementBulkResult), StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateDisbursementBulk([FromBody] CreateDisbursementBulkArgs args)
    {
        try
        {
            var dto = mapper.Map<DisbursementBulkDTO>(args.DisbursementBulk);

            var result = await disbursementRepository.CreateDisbursementBulk(dto);
            if(!result.Succeeded || result.Result is null)
            {
                return new JsonResult(new CreateDisbursementBulkResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new CreateDisbursementBulkResult { IsSuccess = true, Result = result.Result });
        }
        catch (Exception ex)
        {
            return new JsonResult(new CreateDisbursementBulkResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("UpdateDisbursementBulkStatus")]
    [HttpPost]
    [ProducesResponseType(typeof(UpdateDisbursementBulkStatusResult), StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateDisbursementBulkStatus([FromBody] UpdateDisbursementBulkStatusArgs args)
    {
        try
        {
            var result = await disbursementRepository.UpdateDisbursementBulkStatus(args.DisbursementBulkId,
                args.DisbursementBulkStatus, args.DisbursementStatus, args.Remarks ?? string.Empty);
            if(!result.Succeeded || result.Result is null)
            {
                return new JsonResult(new UpdateDisbursementBulkStatusResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new UpdateDisbursementBulkStatusResult { IsSuccess = true, Result = result.Result });
        }
        catch (Exception ex)
        {
            return new JsonResult(new UpdateDisbursementBulkStatusResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("CreateDisbursementBulkLog")]
    [HttpPost]
    [ProducesResponseType(typeof(CreateDisbursementBulkLogResult), StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateDisbursementBulkLog([FromBody] CreateDisbursementBulkLogArgs args)
    {
        try
        {
            var dto = mapper.Map<DisbursementBulkLogDTO>(args.DisbursementBulkLog);

            var result = await disbursementRepository.CreateDisbursementBulkLog(dto);
            if(!result.Succeeded || result.Result is null)
            {
                return new JsonResult(new CreateDisbursementBulkLogResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new CreateDisbursementBulkLogResult { IsSuccess = true, Result = result.Result });
        }
        catch (Exception ex)
        {
            return new JsonResult(new CreateDisbursementBulkLogResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [HttpGet]
    [Route("DisbursmentBulks/{id}")]
    [ProducesResponseType(typeof(GetDisbursementBulkResult), StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetDisbursementBulk(int id)
    {
        try
        {
            var result = await disbursementRepository.GetDisbursementBulk(id);
            if(!result.Succeeded || result.Result is null)
            {
                return new JsonResult(new GetDisbursementBulkResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }
            
            return new JsonResult(new GetDisbursementBulkResult { IsSuccess = true, Result = result.Result });
        }
        catch (Exception ex)
        {
            return new JsonResult(new GetDisbursementBulkResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("GetDisbursementInformation")]
    [HttpGet]
    [ProducesResponseType(typeof(GetDisbursementInformationResult), StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetDisbursementInformation([FromQuery] GetDisbursementInformationArgs args)
    {
        try
        {
            var result = await disbursementRepository.GetDisbursementsInformation(args.FilterBy ?? string.Empty, args.FilterValue ?? string.Empty);
            if(!result.Succeeded || result.Result is null)
            {
                return new JsonResult(new GetDisbursementInformationResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new GetDisbursementInformationResult { IsSuccess = true, Result = result.Result });
        }
        catch (Exception ex)
        {
            return new JsonResult(new GetDisbursementInformationResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("GetDisbursementDetails/{disbursementId}")]
    [HttpGet]
    [ProducesResponseType(typeof(GetDisbursementDetailsResult), StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetDisbursementDetails(int disbursementId)
    {
        try
        {
            var result = await disbursementRepository.GetDisbursementDetails(disbursementId);
            if(!result.Succeeded || result.Result is null)
            {
                return new JsonResult(new GetDisbursementDetailsResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new GetDisbursementDetailsResult { IsSuccess = true, Result = result.Result });
        }
        catch (Exception ex)
        {
            return new JsonResult(new GetDisbursementDetailsResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }
}