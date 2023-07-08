using Cinnamon.Api.Data.Services.Repository.ChatHistory;
using Cinnamon.Api.Data.Services.Repository.Interfaces;
using Cinnamon.Framework.ApiCommand.ApiData;
using Cinnamon.Framework.ApiCommand.ApiData.Favorite.Request;
using Cinnamon.Framework.ApiCommand.ApiData.Favorite.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Cinnamon.Api.Data.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FavoriteController : ControllerBase
    {
        private readonly IFavoriteRepository _favoriteRepository;
        public FavoriteController(IFavoriteRepository favoriteRepository)
        {
            _favoriteRepository = favoriteRepository;
        }

        [Route("Create")]
        [HttpPost]
        [ProducesResponseType(typeof(CreateFavoriteResult), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateFavorite([FromBody] CreateFavoriteArgs args)
        {
            try
            {
                var result = await _favoriteRepository.Create(args.CustomerId,args.ActivityId);

                if (!result.Succeeded || result.Result == null)
                {
                    return new JsonResult(new CreateFavoriteResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
                }

                return new JsonResult(new CreateFavoriteResult { IsSuccess = true });
            }
            catch (Exception ex)
            {
                return new JsonResult(new CreateFavoriteResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
            }
        }

        [Route("Remove")]
        [HttpPost]
        [ProducesResponseType(typeof(RemoveFavoriteResult), StatusCodes.Status200OK)]
        public async Task<IActionResult> RemoveFavorite([FromBody] RemoveFavoriteArgs args)
        {
            try
            {
                var result = await _favoriteRepository.Remove(args.CustomerId, args.ActivityId);

                if (!result.Succeeded || !result.Result)
                {
                    return new JsonResult(new RemoveFavoriteResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
                }

                return new JsonResult(new RemoveFavoriteResult { IsSuccess = true });
            }
            catch (Exception ex)
            {
                return new JsonResult(new RemoveFavoriteResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
            }
        }

        [Route("ByCustomer")]
        [HttpGet]
        [ProducesResponseType(typeof(GetFavoritesByCustomerResult), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetFavoritesByCustomer([FromQuery] GetFavoritesByCustomerArgs args)
        {
            try
            {
                var result = await _favoriteRepository.GetFavoritesByCustomer(args.CustomerId);

                if (!result.Succeeded || result.Result == null)
                {
                    return new JsonResult(new GetFavoritesByCustomerResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
                }

                return new JsonResult(new GetFavoritesByCustomerResult
                {
                    Result = result.Result,
                    IsSuccess = true
                });
            }
            catch (Exception ex)
            {
                return new JsonResult(new GetFavoritesByCustomerResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
            }
        }
    }
}
