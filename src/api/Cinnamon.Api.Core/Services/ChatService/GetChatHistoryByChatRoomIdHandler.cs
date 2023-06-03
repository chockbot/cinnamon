using Cinnamon.Api.Core.Modules.DataAccess.AdminUser;
using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Services.AdminService.Handlers;
using Cinnamon.Api.Core.Services.AdminService.Interactors;
using Cinnamon.Api.Core.Services.AdminService.Interactors.Results;
using Cinnamon.Api.Core.Services.ChatService.Handlers;
using Cinnamon.Api.Core.Services.ChatService.Interactors;
using Cinnamon.Api.Core.Services.ChatService.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.ChatService
{
    public class GetChatHistoryByChatRoomIdHandler : IGetChatHistoryByChatRoomIdHandler
    {
        private readonly IChatHistoryData chatHistoryData;

        public GetChatHistoryByChatRoomIdHandler(IChatHistoryData chatHistoryData)
        {
            this.chatHistoryData = chatHistoryData;
        }

        public AppResult<GetChatHistoryByChatRoomIdResult> Execute(GetChatHistoryByChatRoomIdArgs args)
        {
            try
            {
                return ExecuteAsync(args).Result;
            }
            catch (Exception ex)
            {
                return AppResult<GetChatHistoryByChatRoomIdResult>.CreateFailed(ex, "An error occured in GetChatHistoryByChatRoomIdHandler");
            }
        }

        public async Task<AppResult<GetChatHistoryByChatRoomIdResult>> ExecuteAsync(GetChatHistoryByChatRoomIdArgs args)
        {
            try
            {
                var result = await chatHistoryData.GetChatHistoryByChatRoomId(new Framework.ApiCommand.ApiCore.ChatHistory.Request.GetChatHistoryByChatRoomIdArgs
                {
                    ChatRoomId   = args.ChatRoomId,
                    CountPerPage = args.CountPerPage,
                    PageIndex    = args.PageIndex,
                });

                if (!result.Succeeded || result.Result == null)
                {
                    return AppResult<GetChatHistoryByChatRoomIdResult>.CreateFailed(new ApplicationException(result.Message), result.Message);
                }

                if (result.Succeeded && !result.Result.IsSuccess)
                {
                    return AppResult<GetChatHistoryByChatRoomIdResult>.CreateFailed(
                        new ApplicationException(result.Result.ErrorInfo?.Message), "An error occured in GetChatHistoryByChatRoomIdHandler");
                }

                var chatResult = result.Result.Result;

                return AppResult<GetChatHistoryByChatRoomIdResult>.CreateSucceeded(new GetChatHistoryByChatRoomIdResult
                {
                    ChatHistories = chatResult.Select(c => new ChatHistory
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
                        ToProfilePath    = c.ToProfilePath
                    }),
                    ErrorInfo = new Framework.ApiCommand.ApiCore.ErrorInfo
                    {
                        Code        = result?.Result?.ErrorInfo?.Code,
                        Description = result?.Result?.ErrorInfo?.Description,
                        Message     = result?.Result?.ErrorInfo?.Message
                    },
                    Pagination = new Framework.ApiCommand.ApiCore.Pagination
                    {
                        PageIndex    = result.Result.Pagination.PageIndex,
                        PerPage      = result.Result.Pagination.PerPage,
                        TotalPages   = result.Result.Pagination.TotalPages,
                        TotalRecords = result.Result.Pagination.TotalRecords
                    }
                }, "successfully called GetChatHistoryByChatRoomIdHandler");

            }
            catch (Exception ex)
            {
                return AppResult<GetChatHistoryByChatRoomIdResult>.CreateFailed(ex, "An error occured in GetChatHistoryByChatRoomIdHandler");
            }
        }
    }
}
