using Cinnamon.Api.Core.Services.ActivityService;
using Cinnamon.Api.Core.Services.AdminService.Handlers;
using Cinnamon.Api.Core.Services.ChatService.Handlers;
using Cinnamon.Framework.ApiCommand.ApiCore;
using Cinnamon.Framework.ApiCommand.ApiCore.AdminUser.Request;
using Cinnamon.Framework.ApiCommand.ApiCore.AdminUser.Response;
using Cinnamon.Framework.ApiCommand.ApiCore.ChatHistory.Request;
using Cinnamon.Framework.ApiCommand.ApiCore.ChatHistory.Response;
using Cinnamon.Framework.ApiCommand.ApiCore.ChatRoom.Request;
using Cinnamon.Framework.ApiCommand.ApiCore.ChatRoom.Response;
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
        private readonly IGetChatHistoryByChatRoomIdHandler getChatHistoryByChatRoomIdHandler;
        private readonly ICreateChatRoomHandler createChatRoomHandler;
        private readonly IGetChatRoomsByUserIdHandler getChatRoomsByUserIdHandler;

        public ChatController(ICreateChatHistoryHandler createChatHistoryHandler, ILogger logger, IUpdateChatHistoryHandler updateChatHistoryHandler, IGetChatHistoryByChatRoomIdHandler getChatHistoryByChatRoomIdHandler, ICreateChatRoomHandler createChatRoomHandler, IGetChatRoomsByUserIdHandler getChatRoomsByUserIdHandler)
        {
            _logger = logger;

            this.createChatHistoryHandler = createChatHistoryHandler;
            this.updateChatHistoryHandler = updateChatHistoryHandler;
            this.getChatHistoryByChatRoomIdHandler = getChatHistoryByChatRoomIdHandler;
            this.createChatRoomHandler = createChatRoomHandler;
            this.getChatRoomsByUserIdHandler = getChatRoomsByUserIdHandler;
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

        [Route("ByChatRoomId")]
        [HttpGet]
        [ProducesResponseType(typeof(GetChatHistoryByChatRoomIdResult), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetChatHistoryByChatRoomId([FromQuery] GetChatHistoryByChatRoomIdArgs args)
        {
            try
            {
                var result = await getChatHistoryByChatRoomIdHandler.ExecuteAsync(new Services.ChatService.Interactors.GetChatHistoryByChatRoomIdArgs
                {
                    ChatRoomId = args.ChatRoomId,
                    CountPerPage = args.CountPerPage,
                    PageIndex = args.PageIndex,
                });

                if (!result.Succeeded || result.Result == null)
                {
                    return new JsonResult(new GetChatHistoryByChatRoomIdResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
                }

                var chatResult = result.Result;

                return new JsonResult(new GetChatHistoryByChatRoomIdResult
                {
                    IsSuccess = true,
                    Pagination = result.Result.Pagination,
                    ErrorInfo = result.Result.ErrorInfo,
                    Result = result.Result.ChatHistories.Select(c =>
                    {
                        return new Framework.ApiCommand.ApiData.DTO.ChatHistory.ChatHistoryDTO
                        {
                            ChatRoomId = c.ChatRoomId,
                            DateCreated = c.DateCreated,
                            FromConnectionId = c.FromConnectionId,
                            FromUserId = c.FromUserId,
                            IsViewed = c.IsViewed,
                            Message = c.Message,
                            ToConnectionId = c.ToConnectionId,
                            ToUserId = c.ToUserId,
                        };
                    })
                });
            }
            catch (Exception ex)
            {
                return new JsonResult(new GetChatHistoryByChatRoomIdResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
            }
        }

        [Route("Room/Create")]
        [HttpPost]
        [ProducesResponseType(typeof(CreateChatRoomResult), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateChatRoom([FromBody] CreateChatRoomArgs args)
        {
            try
            {
                var result = await createChatRoomHandler.ExecuteAsync(new Services.ChatService.Interactors.CreateChatRoomArgs
                {
                    FromUserId = args.FromUserId,
                    ToUserId = args.ToUserId
                });

                if (!result.Succeeded || result.Result == null)
                {
                    return new JsonResult(new CreateChatRoomResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
                }

                var chatResult = result.Result;

                return new JsonResult(new CreateChatRoomResult
                {
                    IsSuccess = true,
                    Result = new Framework.ApiCommand.ApiCore.DTO.ChatRoom.ChatRoomDTO
                    {
                        ChatRoomId = chatResult.ChatRoomId,
                        FromUserId = chatResult.FromUserId,
                        ToUserId = chatResult.ToUserId
                    }
                });
            }
            catch (Exception ex)
            {
                return new JsonResult(new CreateChatRoomResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
            }
        }

        [Route("ChatRooms")]
        [HttpGet]
        [ProducesResponseType(typeof(GetChatRoomsByUserIdResult), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetChatRoomsByUserId([FromQuery] GetChatRoomsByUserIdArgs args)
        {
            try
            {
                var result = await getChatRoomsByUserIdHandler.ExecuteAsync(new Services.ChatService.Interactors.GetChatRoomsByUserIdArgs
                {
                    UserId = args.UserId
                });

                if (!result.Succeeded || result.Result == null)
                {
                    return new JsonResult(new GetChatRoomsByUserIdResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
                }

                var chatResult = result.Result;

                return new JsonResult(new GetChatRoomsByUserIdResult
                {
                    IsSuccess = true,
                    Result = result.Result.ChatRooms.Select(c =>
                    {
                        return new Framework.ApiCommand.ApiCore.DTO.ChatRoom.ChatRoomDTO
                        {
                            ChatRoomId    = c.ChatRoomId,
                            FromUserId    = c.FromUserId,
                            DateCreated   = c.DateCreated,
                            FromFirstName = c.FromFirstName,
                            FromLastName  = c.FromLastName,
                            Message       = c.Message,
                            ToFirstName   = c.ToFirstName,
                            ToLastName    = c.ToLastName,
                            ToUserId      = c.ToUserId,
                        };
                    })
                });
            }
            catch (Exception ex)
            {
                return new JsonResult(new GetChatRoomsByUserIdResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
            }
        }
    }
}
