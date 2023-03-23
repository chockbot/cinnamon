using Cinnamon.Api.Data.Services.Repository.Interfaces;
using Cinnamon.Framework.ApiCommand.ApiData;
using Cinnamon.Framework.ApiCommand.ApiData.BadgeList.Request;
using Cinnamon.Framework.ApiCommand.ApiData.BadgeList.Response;
using Microsoft.AspNetCore.Mvc;
namespace Cinnamon.Api.Data.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BadgeController : ControllerBase
    {
        private readonly IBadgeListRepository _badgeListRepository;
        public BadgeController(IBadgeListRepository badgeListRepository)
        {
            _badgeListRepository = badgeListRepository; 
        }
        [Route("GetBadgeById/{id}")]
        [HttpGet]
        [ProducesResponseType(typeof(GetBadgeResult), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetBadgeById(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return NotFound();
                }

                var result = await _badgeListRepository.GetByIdAsync(id);
                if (!result.Succeeded)
                {
                    return new JsonResult(new GetBadgeResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
                }

                if (result.Result == null)
                {
                    return NotFound();
                }

                return new JsonResult(new GetBadgeResult { Result = result.Result, IsSuccess = true });
            }
            catch (Exception ex)
            {
                return new JsonResult(new GetBadgeResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
            }
        }

        [HttpGet]
        [Route("GetAllBadge")]
        [ProducesResponseType(typeof(GetAllBadgeResult), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllBadge()
        {
            try
            {
                var result = await _badgeListRepository.GetAllAsync();
                if (!result.Succeeded)
                {
                    return new JsonResult(new GetAllBadgeResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
                }
                return new JsonResult(new GetAllBadgeResult { Result = result.Result, IsSuccess = true });
            }
            catch (Exception ex)
            {
                return new JsonResult(new GetAllBadgeResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
            }
        }

        [Route("CreateBadge")]
        [HttpPost]
        [ProducesResponseType(typeof(CreateBadgeResult), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateBadge(CreateBadgeArgs args)
        {
            try
            {
                var result = await _badgeListRepository.CreateBadgeAsync(args.Name, args.Description,args.NumberOfStudent, args.NumberOfCompleted, args.NumberOfReviews, args.ImgScr);
                if (!result.Succeeded)
                {
                    return new JsonResult(new CreateBadgeResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
                }
                return new JsonResult(new CreateBadgeResult { IsSuccess = true, Result = result.Result });
            }
            catch (Exception ex)
            {
                return new JsonResult(new CreateBadgeResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
            }
        }

        [Route("UpdateBadge")]
        [HttpPost]
        [ProducesResponseType(typeof(UpdateBadgeResult), StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateExperienceType(UpdateBadgeArgs args)
        {
            try
            {
                var result = await _badgeListRepository.UpdateBadgeAsync(args.Id, args.Name, args.Description, args.NumberOfStudent, args.NumberOfCompleted, args.NumberOfReviews, args.ImgScr);
                if (!result.Succeeded)
                {
                    return new JsonResult(new UpdateBadgeResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
                }
                return new JsonResult(new UpdateBadgeResult { IsSuccess = true, Result = result.Result });
            }
            catch (Exception ex)
            {
                return new JsonResult(new UpdateBadgeResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
            }
        }
    }
}
