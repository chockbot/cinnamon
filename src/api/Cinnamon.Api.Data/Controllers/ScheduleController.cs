using Cinnamon.Api.Data.Services.Repository.Interfaces;
using Cinnamon.Framework.ApiCommand.ApiData;
using Cinnamon.Framework.ApiCommand.ApiData.DTO.Schedule;
using Cinnamon.Framework.ApiCommand.ApiData.Schedule.Request;
using Cinnamon.Framework.ApiCommand.ApiData.Schedule.Response;
using Microsoft.AspNetCore.Mvc;

namespace Cinnamon.Api.Data.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScheduleController : ControllerBase
    {
        private readonly IScheduleRepository _scheduleRepository;
        public ScheduleController(IScheduleRepository scheduleRepository)
        {
            _scheduleRepository = scheduleRepository;
        }

        [Route("GetScheduleById/{id}")]
        [HttpGet]
        [ProducesResponseType(typeof(GetScheduleResult), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetScheduleById(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return NotFound();
                }

                var result = await _scheduleRepository.GetByIdAsync(id);
                if (!result.Succeeded)
                {
                    return new JsonResult(new GetScheduleResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
                }

                if (result.Result == null)
                {
                    return NotFound();
                }

                return new JsonResult(new GetScheduleResult { Result = result.Result, IsSuccess = true });
            }
            catch (Exception ex)
            {
                return new JsonResult(new GetScheduleResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
            }
        }

        [HttpGet]
        [Route("GetAllSchedule")]
        [ProducesResponseType(typeof(GetAllScheduleResult), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllScheduleAsync()
        {
            try
            {
                var result = await _scheduleRepository.GetAllAsync();
                if (!result.Succeeded)
                {
                    return new JsonResult(new GetAllScheduleResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
                }
                return new JsonResult(new GetAllScheduleResult { Result = result.Result, IsSuccess = true });
            }
            catch (Exception ex)
            {
                return new JsonResult(new GetAllScheduleResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
            }
        }

        [Route("CreateSchedule")]
        [HttpPost]
        [ProducesResponseType(typeof(CreateScheduleResult), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateScheduleAsync(CreateScheduleArgs scheduleArgs)
        {
            try
            {
                var result = await _scheduleRepository.CreateSchedule(scheduleArgs.ActivityId,scheduleArgs.Name,scheduleArgs.DateTime,
                                                                      scheduleArgs.Price, scheduleArgs.UnitPrice, scheduleArgs.PerUnit1,
                                                                      scheduleArgs.PriceUnit1, scheduleArgs.PerUnit2, scheduleArgs.PriceUnit2);
                if (!result.Succeeded)
                {
                    return new JsonResult(new CreateScheduleResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
                }
                return new JsonResult(new CreateScheduleResult { IsSuccess = true, Result = result.Result });
            }
            catch (Exception ex)
            {
                return new JsonResult(new CreateScheduleResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
            }
        }

        [Route("UpdateSchedule")]
        [HttpPost]
        [ProducesResponseType(typeof(UpdateScheduleResult), StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateScheduleAsync(UpdateScheduleArgs updateScheduleArgs)
        {
            try
            {
                var result = await _scheduleRepository.UpdateSchedule(updateScheduleArgs.Id, updateScheduleArgs.Name,updateScheduleArgs.DateTime,
                                                                      updateScheduleArgs.Price, updateScheduleArgs.UnitPrice, updateScheduleArgs.PerUnit1,
                                                                      updateScheduleArgs.PriceUnit1, updateScheduleArgs.PerUnit2, updateScheduleArgs.PriceUnit2);
                if (!result.Succeeded)
                {
                    return new JsonResult(new UpdateScheduleResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
                }
                return new JsonResult(new UpdateScheduleResult { IsSuccess = true, Result = result.Result });
            }
            catch (Exception ex)
            {
                return new JsonResult(new UpdateScheduleResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
            }
        }

        [Route("CreateManySchedules")]
        [HttpPost]
        [ProducesResponseType(typeof(CreateManySchedulesResult), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateManySchedules([FromBody] CreateManySchedulesArgs args)
        {
            try
            {
                var schedules =  args.Schedules.Select(s => {
                    return new ScheduleDTO {
                        ActivityId = args.ActivityId,
                        DateTime = s.DateTime,
                        Name = s.Name,
                        PerUnit1 = s.PerUnit1,
                        PerUnit2 = s.PerUnit2,
                        Price = s.Price,
                        PriceUnit1 = s.PriceUnit1,
                        PriceUnit2 = s.PriceUnit2,
                        UnitPrice = s.UnitPrice
                    };
                });

                var result = await _scheduleRepository.CreateSchedules(args.ActivityId, schedules);
                if (!result.Succeeded || result.Result == null)
                {
                    return new JsonResult(new CreateManySchedulesResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
                }

                return new JsonResult(new CreateManySchedulesResult { IsSuccess = true, Result = result.Result });
            }
            catch (Exception ex)
            {
                return new JsonResult(new CreateManySchedulesResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
            }
        }
    }
}
