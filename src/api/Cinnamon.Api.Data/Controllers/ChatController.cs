using AutoMapper;
using Cinnamon.Api.Data.Services.Repository.Customer;
using Cinnamon.Api.Data.Services.Repository.Interfaces;
using Cinnamon.Framework.ApiCommand.ApiData;
using Cinnamon.Framework.ApiCommand.ApiData.ChatConnection.Request;
using Cinnamon.Framework.ApiCommand.ApiData.ChatConnection.Response;
using Cinnamon.Framework.ApiCommand.ApiData.ChatHistory.Request;
using Cinnamon.Framework.ApiCommand.ApiData.ChatHistory.Response;
using Cinnamon.Framework.ApiCommand.ApiData.ChatRoom.Request;
using Cinnamon.Framework.ApiCommand.ApiData.ChatRoom.Response;
using Cinnamon.Framework.ApiCommand.ApiData.Customer.Request;
using Cinnamon.Framework.ApiCommand.ApiData.Customer.Response;
using Cinnamon.Framework.ApiCommand.ApiData.DTO.ChatUnreadNotification;
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
        private readonly IChatRoomRepository _chatRoomRepository;
        private readonly IChatMemberRepository _chatMemberRepository;
        private readonly IChatConnectionRepository _chatConnectionRepository;
        private readonly IChatUnreadNotificationRepository chatUnreadNotificationRepository;
        private readonly IMapper mapper;

        public ChatController(IChatHistoryRepository chatHistoryRepository, IChatRoomRepository chatRoomRepository, 
            IChatMemberRepository chatMemberRepository, IChatConnectionRepository chatConnectionRepository,
            IChatUnreadNotificationRepository chatUnreadNotificationRepository, IMapper mapper)
        {
            _chatHistoryRepository = chatHistoryRepository;
            _chatRoomRepository = chatRoomRepository;
            _chatMemberRepository = chatMemberRepository;
            _chatConnectionRepository = chatConnectionRepository;
            this.chatUnreadNotificationRepository = chatUnreadNotificationRepository;
            this.mapper = mapper;
        }

        [Route("Create")]
        [HttpPost]
        [ProducesResponseType(typeof(CreateChatHistoryResult), StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateChatHistory([FromBody] CreateChatHistoryArgs args)
        {
            try
            {
                var result = await _chatHistoryRepository.Create(args.ChatRoomId, args.FromUserId, args.ToUserId, args.Message, args.IsViewed, args.FromConnectionId, args.ToConnectionId, args.ChatHistoryType);

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

        [Route("Update")]
        [HttpPost]
        [ProducesResponseType(typeof(UpdateChatHistoryResult), StatusCodes.Status201Created)]
        public async Task<IActionResult> UpdateChatHistory([FromBody] UpdateChatHistoryArgs args)
        {
            try
            {
                var result = await _chatHistoryRepository.Update(args.ChatRoomId, args.FromUserId, args.ToUserId, args.IsViewed);

                if (!result.Succeeded || result.Result == false)
                {
                    return new JsonResult(new UpdateChatHistoryResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
                }

                return new JsonResult(new UpdateChatHistoryResult { IsSuccess = true });
            }
            catch (Exception ex)
            {
                return new JsonResult(new UpdateChatHistoryResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
            }
        }

        [Route("ChatHistories")]
        [HttpGet]
        [ProducesResponseType(typeof(GetChatHistoryByChatRoomIdResult), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetChatHistoryByChatRoomId([FromQuery] GetChatHistoryByChatRoomIdArgs args)
        {
            try
            {
                int? skip = 0;
                int? take = args.CountPerPage;

                skip = (args.PageIndex - 1) * args.CountPerPage;

                var result = await _chatHistoryRepository.GetChatHistoryByChatRoomId(args.ChatRoomId, skip, take, args.UserId);

                if (!result.Succeeded || result.Result == null)
                {
                    return new JsonResult(new GetChatHistoryByChatRoomIdResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
                }

                var totalRecords = result.Result.Count();
                return new JsonResult(new GetChatHistoryByChatRoomIdResult
                {
                    Result = result.Result,
                    IsSuccess = true,
                    Pagination = new Pagination
                    {
                        PageIndex = args.PageIndex,
                        PerPage = args.CountPerPage,
                        TotalRecords = totalRecords,
                        TotalPages = args.CountPerPage.HasValue && args.PageIndex.HasValue ?
                                    (int)Math.Ceiling((double)totalRecords / args.CountPerPage.Value) : null
                    }
                });
            }
            catch (Exception ex)
            {
                return new JsonResult(new GetChatHistoryByChatRoomIdResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
            }
        }

        [Route("Room/Create")]
        [HttpPost]
        [ProducesResponseType(typeof(CreateChatRoomResult), StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateChatRoom([FromBody] CreateChatRoomArgs args)
        {
            try
            {
                var result = await _chatRoomRepository.Create(args.FromUserId, args.ToUserId, args.ChatType, args.GroupName, args.ChatName);

                if (!result.Succeeded || result.Result == null)
                {
                    return new JsonResult(new CreateChatRoomResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
                }

                return new JsonResult(new CreateChatRoomResult { IsSuccess = true, Result = result.Result });
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
                var result = await _chatMemberRepository.GetChatRoomsByUserId(args.UserId);

                if (!result.Succeeded || result.Result == null)
                {
                    return new JsonResult(new GetChatRoomsByUserIdResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
                }

                var totalRecords = result.Result.Count();
                return new JsonResult(new GetChatRoomsByUserIdResult
                {
                    Result = result.Result,
                    IsSuccess = true,
                });
            }
            catch (Exception ex)
            {
                return new JsonResult(new GetChatRoomsByUserIdResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
            }
        }

        [Route("ChatMembers")]
        [HttpGet]
        [ProducesResponseType(typeof(GetChatMembersByChatRoomIdResult), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetChatMembersByChatRoomId([FromQuery] GetChatMembersByChatRoomIdArgs args)
        {
            try
            {
                var result = await _chatMemberRepository.GetChatMembersByChatRoomId(args.ChatRoomId, args.UserId, args.HasLeft);

                if (!result.Succeeded || result.Result == null)
                {
                    return new JsonResult(new GetChatMembersByChatRoomIdResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
                }

                var totalRecords = result.Result.Count();
                return new JsonResult(new GetChatMembersByChatRoomIdResult
                {
                    Result = result.Result,
                    IsSuccess = true,
                });
            }
            catch (Exception ex)
            {
                return new JsonResult(new GetChatMembersByChatRoomIdResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
            }
        }

        [Route("ChatMember/Update")]
        [HttpPost]
        [ProducesResponseType(typeof(UpdateChatMemberResult), StatusCodes.Status201Created)]
        public async Task<IActionResult> UpdateChatMember([FromBody] UpdateChatMemberArgs args)
        {
            try
            {
                var result = await _chatMemberRepository.UpdateChatMember(args.ChatRoomId, args.UserId, args.HasLeft);

                if (!result.Succeeded || !result.Result)
                {
                    return new JsonResult(new CreateChatRoomResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
                }

                return new JsonResult(new CreateChatRoomResult { IsSuccess = result.Result });
            }
            catch (Exception ex)
            {
                return new JsonResult(new CreateChatRoomResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
            }
        }

        [Route("ChatConnection")]
        [HttpPost]
        [ProducesResponseType(typeof(CreateChatConnectionResult), StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateChatConnection([FromBody] CreateChatConnectionArgs args)
        {
            try
            {
                var result = await _chatConnectionRepository.Create(args.CustomerId, args.ConnectionId, args.UserAgent, args.IsConnected);

                if (!result.Succeeded || !result.Result)
                {
                    return new JsonResult(new CreateChatConnectionResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
                }

                return new JsonResult(new CreateChatConnectionResult { IsSuccess = result.Result });
            }
            catch (Exception ex)
            {
                return new JsonResult(new CreateChatConnectionResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
            }
        }

        [Route("ChatConnection/Update")]
        [HttpPost]
        [ProducesResponseType(typeof(UpdateChatConnectionResult), StatusCodes.Status201Created)]
        public async Task<IActionResult> UpdateChatConnection([FromBody] UpdateChatConnectionArgs args)
        {
            try
            {
                var result = await _chatConnectionRepository.Update(args.CustomerId, args.ConnectionId, args.IsConnected);

                if (!result.Succeeded || !result.Result)
                {
                    return new JsonResult(new UpdateChatConnectionResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
                }

                return new JsonResult(new UpdateChatConnectionResult { IsSuccess = result.Result });
            }
            catch (Exception ex)
            {
                return new JsonResult(new UpdateChatConnectionResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
            }
        }

        [Route("ChatConnections")]
        [HttpGet]
        [ProducesResponseType(typeof(GetChatConnectionByCustomerResult), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetChatConnectionByCustomer([FromQuery] GetChatConnectionByCustomerArgs args)
        {
            try
            {
                var result = await _chatConnectionRepository.GetChatConnectionByCustomer(args.CustomerId);

                if (!result.Succeeded || result.Result == null)
                {
                    return new JsonResult(new GetChatConnectionByCustomerResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
                }

                return new JsonResult(new GetChatConnectionByCustomerResult
                {
                    Result = result.Result,
                    IsSuccess = true,
                });
            }
            catch (Exception ex)
            {
                return new JsonResult(new GetChatConnectionByCustomerResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
            }
        }

        [Route("GetUnreadMessages")]
        [HttpGet]
        [ProducesResponseType(typeof(GetUnreadMessagesResult), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUnreadMessages()
        {
            try
            {
                var result = await chatUnreadNotificationRepository.GetUnreadMessages();

                if (!result.Succeeded || result.Result == null)
                {
                    return new JsonResult(new GetUnreadMessagesResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
                }

                return new JsonResult(new GetUnreadMessagesResult
                {
                    Result = result.Result,
                    IsSuccess = true,
                });
            }
            catch (Exception ex)
            {
                return new JsonResult(new GetUnreadMessagesResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
            }
        }

        [Route("CreateUnreadNotification")]
        [HttpPost]
        [ProducesResponseType(typeof(CreateUnreadNotificationResult), StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateUnreadNotification([FromBody] CreateUnreadNotificationArgs args)
        {
            try
            {
                var dto = mapper.Map<ChatUnreadNotificationDTO>(args);

                var result = await chatUnreadNotificationRepository.CreateChatUnreadNotification(dto);
                if (!result.Succeeded || result.Result is null)
                {
                    return new JsonResult(new CreateUnreadNotificationResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
                }

                return new JsonResult(new CreateUnreadNotificationResult { IsSuccess = true, Result = result.Result });
            }
            catch (Exception ex)
            {
                return new JsonResult(new CreateUnreadNotificationResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
            }
        }

        [Route("Room/UpdateName")]
        [HttpPost]
        [ProducesResponseType(typeof(UpdateChatRoomNameResult), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateChatRoom([FromBody] UpdateChatRoomArgs args)
        {
            try
            {
                var result = await _chatRoomRepository.UpdateChatRoomName(args.ChatRoomId, args.NewChatRoomName);

                if (!result.Succeeded || result.Result is null)
                {
                    return new JsonResult(new UpdateChatRoomNameResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
                }

                return new JsonResult(new UpdateChatRoomNameResult { IsSuccess = result.Succeeded });
            }
            catch (Exception ex)
            {
                return new JsonResult(new UpdateChatRoomNameResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
            }
        }
    }
}
    