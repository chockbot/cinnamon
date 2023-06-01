using Cinnamon.Api.Data.Services.Repository.Customer;
using Cinnamon.Api.Data.Services.Repository.Interfaces;
using Cinnamon.Framework.ApiCommand.ApiData;
using Cinnamon.Framework.ApiCommand.ApiData.ChatHistory.Request;
using Cinnamon.Framework.ApiCommand.ApiData.ChatHistory.Response;
using Cinnamon.Framework.ApiCommand.ApiData.Customer.Request;
using Cinnamon.Framework.ApiCommand.ApiData.Customer.Response;
using Cinnamon.Framework.ApiCommand.ApiData.Location.Request;
using Cinnamon.Framework.ApiCommand.ApiData.Location.Response;
using Cinnamon.Framework.ApiCommand.ApiData.Student.Response;
using Microsoft.AspNetCore.Mvc;

namespace Cinnamon.Api.Data.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly IChatHistoryRepository _chatHistoryRepository;

        public ChatController(IChatHistoryRepository chatHistoryRepository)
        {
            _chatHistoryRepository = chatHistoryRepository;
        }

        [Route("Create")]
        [HttpPost]
        [ProducesResponseType(typeof(CreateChatHistoryResult), StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateChatHistory([FromBody] CreateChatHistoryArgs args)
        {
            try
            {
                var result = await _chatHistoryRepository.Create(args.ChatRoomId, args.FromUserId, args.ToUserId, args.Message, args.IsViewed, args.FromConnectionId, args.ToConnectionId);

                if (!result.Succeeded || result.Result == null)
                {
                    return new JsonResult(new CreateChatHistoryResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
                }

                return new JsonResult(new CreateChatHistoryResult { IsSuccess = true, Result = result.Result });
            }
            catch (Exception ex)
            {
                return new JsonResult(new CreateChatHistoryResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
            }
        }
    }
}
    