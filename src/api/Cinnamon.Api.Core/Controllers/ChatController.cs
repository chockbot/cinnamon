using Cinnamon.Api.Core.Services.AccountService.Handlers;
using Cinnamon.Api.Core.Services.ActivityService;
using Cinnamon.Api.Core.Services.AdminService.Handlers;
using Cinnamon.Api.Core.Services.ChatService.Handlers;
using Cinnamon.Framework.ApiCommand.ApiCore;
using Cinnamon.Framework.ApiCommand.ApiCore.Account.Request;
using Cinnamon.Framework.ApiCommand.ApiCore.Account.Response;
using Cinnamon.Framework.ApiCommand.ApiCore.AdminUser.Request;
using Cinnamon.Framework.ApiCommand.ApiCore.AdminUser.Response;
using Cinnamon.Framework.ApiCommand.ApiCore.ChatConnection.Request;
using Cinnamon.Framework.ApiCommand.ApiCore.ChatConnection.Response;
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
        private readonly IUpdateConnectionIdHandler updateConnectionIdHandler;
        private readonly IGetChatMembersByChatRoomIdHandler getChatMembersByChatRoomIdHandler;
        private readonly IGetChatConnectionByCustomerHandler getChatConnectionByCustomerHandler;
        private readonly IRequestMessageHandler requestMessageHandler;
        private readonly IGetRequestMessageHandler getRequestMessageHandler;
        private readonly IUpdateChatRoomNameHandler updateChatRoomNameHandler;

        public ChatController(ICreateChatHistoryHandler createChatHistoryHandler, ILogger<ChatController> logger, 
            IUpdateChatHistoryHandler updateChatHistoryHandler, IGetChatHistoryByChatRoomIdHandler getChatHistoryByChatRoomIdHandler, 
            ICreateChatRoomHandler createChatRoomHandler, IGetChatRoomsByUserIdHandler getChatRoomsByUserIdHandler, 
            IUpdateConnectionIdHandler updateConnectionIdHandler, IGetChatMembersByChatRoomIdHandler getChatMembersByChatRoomIdHandler, 
            IGetChatConnectionByCustomerHandler getChatConnectionByCustomerHandler, IRequestMessageHandler requestMessageHandler,
            IGetRequestMessageHandler getRequestMessageHandler, IUpdateChatRoomNameHandler updateChatRoomNameHandler)
        {
            _logger = logger;

            this.createChatHistoryHandler = createChatHistoryHandler;
            this.updateChatHistoryHandler = updateChatHistoryHandler;
            this.getChatHistoryByChatRoomIdHandler = getChatHistoryByChatRoomIdHandler;
            this.createChatRoomHandler = createChatRoomHandler;
            this.getChatRoomsByUserIdHandler = getChatRoomsByUserIdHandler;
            this.updateConnectionIdHandler = updateConnectionIdHandler;
            this.getChatMembersByChatRoomIdHandler = getChatMembersByChatRoomIdHandler;
            this.getChatConnectionByCustomerHandler = getChatConnectionByCustomerHandler;
            this.requestMessageHandler = requestMessageHandler;
            this.getRequestMessageHandler = getRequestMessageHandler;
            this.updateChatRoomNameHandler = updateChatRoomNameHandler;
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
                    ToUserId = args.ToUserId,
                    ChatHistoryType = args.ChatHistoryType
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
                    UserId = args.UserId
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
                            ChatHistoryId    = c.ChatHistoryId,
                            ChatRoomId       = c.ChatRoomId,
                            DateCreated      = c.DateCreated,
                            FromConnectionId = c.FromConnectionId,
                            FromUserId       = c.FromUserId,
                            FromFirstName    = c.FromFirstName,
                            FromLastName     = c.FromLastName,
                            FromProfilePath  = c.FromProfilePath,
                            IsViewed         = c.IsViewed,
                            Message          = c.Message,
                            ToConnectionId   = c.ToConnectionId,
                            ToUserId         = c.ToUserId,
                            ToFirstName      = c.ToFirstName,
                            ToLastName       = c.ToLastName,
                            ToProfilePath    = c.ToProfilePath,
                            ChatHistoryType  = c.ChatHistoryType
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
                    ToUserId = args.ToUserId,
                    ChatType = args.ChatType,
                    GroupName = args.GroupName,
                    ChatName = args.ChatName
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
                        ToUserId = chatResult.ToUserId,
                        GroupName = chatResult.GroupName,
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
                            ChatRoomId       = c.ChatRoomId,
                            FromUserId       = c.FromUserId,
                            DateCreated      = c.DateCreated,
                            FromFirstName    = c.FromFirstName,
                            FromLastName     = c.FromLastName,
                            Message          = c.Message,
                            ToFirstName      = c.ToFirstName,
                            ToLastName       = c.ToLastName,
                            ToUserId         = c.ToUserId,
                            FromProfilePath  = c.FromProfilePath,
                            ToProfilePath    = c.ToProfilePath,
                            FromConnectionId = c.FromConnectionId,
                            ToConnectionId   = c.ToConnectionId,
                            HasNewMessage    = c.HasNewMessage,
                            FromProfileLink  = c.FromProfileLink,
                            ChatName         = c.ChatName,
                            ChatType         = c.ChatType,
                            GroupName        = c.GroupName
                        };
                    })
                });
            }
            catch (Exception ex)
            {
                return new JsonResult(new GetChatRoomsByUserIdResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
            }
        }

        [Route("Connection/Update")]
        [HttpPost]
        [ProducesResponseType(typeof(UpdateConnectionIdResult), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateConnectionId([FromBody] UpdateConnectionIdArgs args)
        {
            try
            {
                var result = await updateConnectionIdHandler.ExecuteAsync(new Services.AccountService.Interactors.UpdateConnectionIdArgs
                {
                    ConnectionId = args.ConnectionId,
                    CustomerId = args.CustomerId,
                });

                if (!result.Succeeded || result.Result == null)
                {
                    return new JsonResult(new UpdateConnectionIdResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
                }

                var updateResult = result.Result;

                return new JsonResult(new UpdateConnectionIdResult
                {
                    IsSuccess = true,
                    Result = new Framework.ApiCommand.ApiCore.DTO.Customer.CustomerDTO
                    {
                        Id = updateResult.CustomerId,
                        ConnectionId= updateResult.ConnectionId
                    }
                });
            }
            catch (Exception ex)
            {
                return new JsonResult(new CreateChatRoomResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
            }
        }

        [Route("ChatMembers")]
        [HttpGet]
        [ProducesResponseType(typeof(GetChatMembersByChatRoomIdResult), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetChatMembersByChatRoomId([FromQuery] GetChatMembersByChatRoomIdArgs args)
        {
            try
            {
                var result = await getChatMembersByChatRoomIdHandler.ExecuteAsync(new Services.ChatService.Interactors.GetChatMembersByChatRoomIdArgs
                {
                    ChatRoomId = args.ChatRoomId,
                    UserId = args.UserId,
                    HasLeft = args.HasLeft
                });

                if (!result.Succeeded || result.Result == null)
                {
                    return new JsonResult(new GetChatMembersByChatRoomIdResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
                }

                var chatResult = result.Result;

                return new JsonResult(new GetChatMembersByChatRoomIdResult
                {
                    IsSuccess = true,
                    Result = chatResult.ChatMembers.Select(c => new Framework.ApiCommand.ApiCore.DTO.ChatRoom.ChatRoomDTO
                    {
                        FromUserId = c.FromUserId,
                        FromFirstName = c.FromFirstName,
                        FromLastName = c.FromLastName,
                        FromProfilePath = c.FromProfilePath,
                        ChatMemberType = c.ChatMemberType,
                        FromProfileLink = c.FromProfileLink,
                        CommonPrivateChatRoomId = c.CommonPrivateChatRoomId,
                        ConnectionIds = c.ConnectionIds
                    })
                });
            }
            catch (Exception ex)
            {
                return new JsonResult(new GetChatHistoryByChatRoomIdResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
            }
        }

        [Route("ChatConnections")]
        [HttpGet]
        [ProducesResponseType(typeof(GetChatConnectionByCustomerResult), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetChatConnectionByCustomer([FromQuery] GetChatConnectionByCustomerArgs args)
        {
            try
            {
                var result = await getChatConnectionByCustomerHandler.ExecuteAsync(new Services.ChatService.Interactors.GetChatConnectionByCustomerArgs
                {
                    CustomerId = args.CustomerId
                });

                if (!result.Succeeded || result.Result == null)
                {
                    return new JsonResult(new GetChatConnectionByCustomerResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
                }

                var chatResult = result.Result;

                return new JsonResult(new GetChatConnectionByCustomerResult
                {
                    IsSuccess = true,
                    Result = chatResult.ChatConnections.Select(c => new Framework.ApiCommand.ApiCore.DTO.ChatConnection.ChatConnectionDTO
                    {
                        ConnectionId = c.ConnectionId,
                        CustomerId = c.CustomerId,
                        IsConnected = c.IsConnected,
                        UserAgent = c.UserAgent
                    })
                });
            }
            catch (Exception ex)
            {
                return new JsonResult(new GetChatConnectionByCustomerResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
            }
        }

        [AllowAnonymous]
        [Route("RequestMessage")]
        [HttpPost]
        [ProducesResponseType(typeof(RequestMessageResult), StatusCodes.Status200OK)]
        public async Task<IActionResult> RequestMessage([FromBody] RequestMessageArgs args)
        {
            try
            {
                var result = await requestMessageHandler.ExecuteAsync(new Services.ChatService.Interactors.RequestMessageArgs {
                    ProviderId = args.ProviderId
                });

                if (!result.Succeeded || result.Result == null)
                {
                    return new JsonResult(new RequestMessageResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
                }

                var updateResult = result.Result;

                return new JsonResult(new RequestMessageResult
                {
                    IsSuccess = true,
                    Result = new Framework.ApiCommand.ApiCore.DTO.ChatRoom.RequestMessageDTO {
                        Guid = result.Result.Guid,
                        Token = result.Result.Token
                    }
                });
            }
            catch (Exception ex)
            {
                return new JsonResult(new RequestMessageResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
            }
        }

        [Route("GetRequestMessage")]
        [HttpGet]
        [ProducesResponseType(typeof(GetRequestMessageResult), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRequestMessage([FromQuery] GetRequestMessageArgs args)
        {
            try
            {
                var result = await getRequestMessageHandler.ExecuteAsync(new Services.ChatService.Interactors.GetRequestMessageArgs {
                    Guid = args.Guid,
                    Token = args.Token
                });

                if (!result.Succeeded || result.Result == null)
                {
                    return new JsonResult(new GetRequestMessageResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
                }

                var payload = result.Result;

                return new JsonResult(new GetRequestMessageResult
                {
                    IsSuccess = true,
                    Result = new Framework.ApiCommand.ApiCore.DTO.ChatRoom.RequestMessagePayloadDTO {
                        FirstName = payload.FirstName,
                        ImageSrc = payload.ImageSrc,
                        LastName = payload.LastName,
                        ProviderId = payload.ProviderId
                    }
                });
            }
            catch (Exception ex)
            {
                return new JsonResult(new GetRequestMessageResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
            }
        }

        [Route("UpdateChatRoomName")]
        [HttpPost]
        [ProducesResponseType(typeof(UpdateChatRoomNameResult), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateChatRoomName([FromBody] UpdateChatRoomNameArgs args)
        {
            try
            {
                var result = await updateChatRoomNameHandler.ExecuteAsync(new Services.ChatService.Interactors.UpdateChatRoomNameArgs
                {
                    ChatRoomId = args.ChatRoomId,
                    ChatRoomName = args.ChatRoomName
                });

                if (!result.Succeeded || result.Result == null)
                {
                    return new JsonResult(new UpdateChatRoomNameResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
                }

                var updateResult = result.Result;

                return new JsonResult(new UpdateChatRoomNameResult
                {
                    IsSuccess = true,
                    Result = updateResult.IsSuccess
                });
            }
            catch (Exception ex)
            {
                return new JsonResult(new UpdateChatRoomNameResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
            }
        }
    }
}
