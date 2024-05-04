using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Services.AccountService.Handlers;
using Cinnamon.Api.Core.Services.AccountService.Interactors;
using Cinnamon.Api.Core.Services.AccountService.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.AccountService
{
    public class GetAllCustomersHandler : IGetAllCustomersHandler
    {
        private readonly ICustomerData customerData;

        public GetAllCustomersHandler(ICustomerData customerData)
        {
            this.customerData = customerData;
        }

        public AppResult<GetAllCustomerResult> Execute(GetAllCustomersArgs args)
        {
            try
            {
                return ExecuteAsync(args).Result;
            }
            catch (Exception ex)
            {
                return AppResult<GetAllCustomerResult>.CreateFailed(ex, "An error occured in GetAllCustomersHandler");
            }
        }

        public async Task<AppResult<GetAllCustomerResult>> ExecuteAsync(GetAllCustomersArgs args)
        {
            var result = await customerData.GetAllCustomers(new Framework.ApiCommand.ApiData.Customer.Request.GetAllCustomersArgs
            {
                SearchValue = args.SearchValue,
                CountPerPage = args.CountPerPage,
                PageIndex = args.PageIndex,
                IsOfficialPartner = args.IsOfficialPartner,
                HasVerification = args.HasVerification
            });
            if (!result.Succeeded || result.Result == null)
            {
                return AppResult<GetAllCustomerResult>.CreateFailed(new ApplicationException(result.Message), result.Message);
            }

            if (result.Succeeded && !result.Result.IsSuccess)
            {
                return AppResult<GetAllCustomerResult>.CreateFailed(
                    new ApplicationException(result.Result.ErrorInfo?.Message), "An error occured in GetAllCustomersHandler");
            }

            return AppResult<GetAllCustomerResult>.CreateSucceeded(new GetAllCustomerResult
            {
                Customers = result.Result.Result.Select(c => new GetAllCustomerResult.Customer
                {
                    About            = c.About,
                    Birthdate        = c.Birthdate,
                    Email            = c.Email,
                    ExternalLogin    = c.ExternalLogin,
                    FirstName        = c.FirstName,
                    LastName         = c.LastName,
                    Id               = c.Id,
                    IsMaker          = c.IsMaker,
                    Handler          = c.Handler,
                    FrontIdImagePath = c.FrontIdImagePath,
                    BackIdImagePath  = c.BackIdImagePath,
                    IsVerified       = c.IsVerified,
                    IsVerifiedDate   = c.IsVerifiedObtainedDate,
                    IsOG             = c.IsOG,
                    IsOGDate         = c.IsOGObtainedDate,
                    IsOF             = c.IsOfficial,
                    IsOFDate         = c.IsOfficialObtainedDate,
                    CustomerPricing = new GetAllCustomerResult.CustomerPricing {
                        Rate = c.CustomerPricing != null ? c.CustomerPricing.Rate : 0,
                        IsManualPayment = c.CustomerPricing != null ? c.CustomerPricing.IsManualPayment : false,
                        InclusivePricing = c.CustomerPricing != null ? c.CustomerPricing.InclusivePricing : false
                    },
                    IsAccountBan     = c.IsAccountBan
                }),
                Pagination = new Framework.ApiCommand.ApiCore.Pagination
                {
                    PageIndex = result.Result.Pagination.PageIndex,
                    PerPage = result.Result.Pagination.PerPage,
                    TotalPages = result.Result.Pagination.TotalPages,
                    TotalRecords = result.Result.Pagination.TotalRecords
                }
            }, "successfully retrieved customers");
        }
    }
}
