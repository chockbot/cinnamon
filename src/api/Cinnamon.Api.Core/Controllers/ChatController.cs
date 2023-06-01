using Cinnamon.Api.Core.Services.ActivityService;
using Cinnamon.Api.Core.Services.AdminService.Handlers;
using Cinnamon.Api.Core.Services.ChatService.Handlers;
using Cinnamon.Framework.ApiCommand.ApiCore;
using Cinnamon.Framework.ApiCommand.ApiCore.AdminUser.Request;
using Cinnamon.Framework.ApiCommand.ApiCore.AdminUser.Response;
using Cinnamon.Framework.ApiCommand.ApiCore.ChatHistory.Request;
using Cinnamon.Framework.ApiCommand.ApiCore.ChatHistory.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Cinnamon.Api.Core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    public class ChatController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly ICreateChatHistoryHandler createChatHistoryHandler;
        private readonly IUpdateChatHistoryHandler updateChatHistoryHandler;

        public ChatController(ICreateChatHistoryHandler createChatHistoryHandler, ILogger logger, IUpdateChatHistoryHandler updateChatHistoryHandler)
        {
            _logger = logger;

            this.createChatHistoryHandler = createChatHistoryHandler;
            this.updateChatHistoryHandler = updateChatHistoryHandler;
        }

        [Route("Create")]
        [HttpPost]
        [ProducesResponseType(typeof(CreateChatHistoryResult), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateChatHistory([FromBody] CreateChatHistoryArgs args)
        {
            try
            {
                var result = await createChatHistoryHandler.ExecuteAsync(new Services.ChatService.Interactors.CreateChatHistoryArgs
                {
                    ChatRoomId = args.ChatRoomId,
                    FromConnectionId = args.FromConnectionId,
                    FromUserId = args.FromUserId,
                    IsViewed = args.IsViewed,
                    Message = args.Message,
                    ToConnectionId = args.ToConnectionId,
                    ToUserId = args.ToUserId
                });

                if (!result.Succeeded || result.Result == null)
                {
                    return new JsonResult(new CreateChatHistoryResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
                }

                var chatResult = result.Result;

                return new JsonResult(new CreateChatHistoryResult
                {
                    IsSuccess = true,
                    Result = new Framework.ApiCommand.ApiData.DTO.ChatHistory.ChatHistoryDTO
                    {
                        ChatRoomId       = chatResult.ChatRoomId,
                        FromConnectionId = chatResult.FromConnectionId,
                        FromUserId       = chatResult.FromUserId,
                        IsViewed         = chatResult.IsViewed,
                        Message          = chatResult.Message,
                        ToConnectionId   = chatResult.ToConnectionId,
                        ToUserId         = chatResult.ToUserId
                    }
                });
            }
            catch (Exception ex)
            {
                return new JsonResult(new CreateChatHistoryResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
            }
        }

        [Route("Update")]
        [HttpPost]
        [ProducesResponseType(typeof(UpdateChatHistoryResult), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateChatHistory([FromBody] UpdateChatHistoryArgs args)
        {
            try
            {
                var result = await updateChatHistoryHandler.ExecuteAsync(new Services.ChatService.Interactors.UpdateChatHistoryArgs
                {
                    ChatRoomId = args.ChatRoomId,
                    FromUserId = args.FromUserId,
                    ToUserId   = args.ToUserId,
                    IsViewed   = args.IsViewed
                });

                if (!result.Succeeded || result.Result == null)
                {
                    return new JsonResult(new UpdateChatHistoryResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
                }

                var chatResult = result.Result;

                return new JsonResult(new UpdateChatHistoryResult
                {
                    IsSuccess = chatResult.IsSuccess
                });
            }
            catch (Exception ex)
            {
                return new JsonResult(new UpdateChatHistoryResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
            }
        }
    }
}
