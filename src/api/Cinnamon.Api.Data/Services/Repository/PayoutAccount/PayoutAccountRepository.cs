using Cinnamon.Api.Data.Repository.Interfaces;
using Cinnamon.Api.Data.Services.Repository.Interfaces;
using Cinnamon.Framework.ApiCommand.ApiData.DTO.PayoutAccount;
using Cinnamon.Framework.Common;
using Entities = Cinnamon.Api.Data.Repository.Entities;

namespace Cinnamon.Api.Data.Services.Repository.PayoutAccount;

public class PayoutAccountRepository : IPayoutAccountRepository
{
    private readonly IDataStore dataStore;
    
    public PayoutAccountRepository(IDataStore dataStore)
    {
        this.dataStore = dataStore;
    }

    public async Task<AppResult<PayoutAccountDTO>> Create(int customerId, string accountNo, string accountHolder, string payload, string bankChannel)
    {
        try
        {
            // check customer id
            var customerChk = await dataStore.Customer.GetByIdAsync(customerId);
            if(!customerChk.Succeeded || customerChk.Result == null)
            {
                return AppResult<PayoutAccountDTO>.CreateFailed(new ApplicationException("Invalid customer id"), "Invalid customer id");
            }

            var entity = new Entities.PayoutAccount {
                AccountNumber = accountNo,
                AccountHolder = accountHolder,
                CustomerId = customerId,
                Payloads = payload,
                BankChannel = bankChannel
            };

            var result = await dataStore.PayoutAccount.Add(entity);
            if(!result.Succeeded || result.Result == null)
            {
                return AppResult<PayoutAccountDTO>.CreateFailed(new ApplicationException(result.Message), result.Message);
            }
            
            return AppResult<PayoutAccountDTO>.CreateSucceeded(new PayoutAccountDTO {
                AccountHolder = result.Result.AccountHolder,
                AccountNumber = result.Result.AccountNumber,
                CustomerId = result.Result.CustomerId,
                Id = result.Result.Id,
                Payload = result.Result.Payloads,
                BankChannel = result.Result.BankChannel
            }, "Successcully created payout account");
        }
        catch (Exception ex)
        {
            return AppResult<PayoutAccountDTO>.CreateFailed(ex, "An error occured when creating payout account");
        }
    }

    public async Task<AppResult<IEnumerable<PayoutAccountDTO>>> GetAllAsync(int? count, int? skip)
    {
        try
        {
            var result = await dataStore.PayoutAccount.FindAsync(p => true, count, skip);
            if(!result.Succeeded || result.Result == null)
            {
                return AppResult<IEnumerable<PayoutAccountDTO>>.CreateFailed(new ApplicationException(result.Message), result.Message);
            }

            return AppResult<IEnumerable<PayoutAccountDTO>>.CreateSucceeded(
                result.Result.Select(p => {
                    return new PayoutAccountDTO {
                        AccountHolder = p.AccountHolder,
                        AccountNumber = p.AccountNumber,
                        CustomerId = p.CustomerId,
                        Id = p.Id,
                        Payload = p.Payloads,
                        BankChannel = p.BankChannel
                    };
                }), "Successfully get all payout accounts");
        }
        catch (Exception ex)
        {
            return AppResult<IEnumerable<PayoutAccountDTO>>.CreateFailed(ex, "An error occured when getting all payout account");
        }
    }

    public async Task<AppResult<IEnumerable<PayoutAccountDTO>>> GetAllAsync()
    {
        try
        {
            var result = await dataStore.PayoutAccount.GetAllAsync();
            if(!result.Succeeded || result.Result == null)
            {
                return AppResult<IEnumerable<PayoutAccountDTO>>.CreateFailed(new ApplicationException(result.Message), result.Message);
            }

            return AppResult<IEnumerable<PayoutAccountDTO>>.CreateSucceeded(
                result.Result.Select(p => {
                    return new PayoutAccountDTO {
                        AccountHolder = p.AccountHolder,
                        AccountNumber = p.AccountNumber,
                        CustomerId = p.CustomerId,
                        Id = p.Id,
                        Payload = p.Payloads,
                        BankChannel = p.BankChannel
                    };
                }), "Successfully get all payout accounts");
        }
        catch (Exception ex)
        {
            return AppResult<IEnumerable<PayoutAccountDTO>>.CreateFailed(ex, "An error occured when getting all payout account");
        }
    }

    public async Task<AppResult<PayoutAccountDTO>> GetByCustomerIdAsync(int id)
    {
        try
        {
            var result = await dataStore.PayoutAccount.FindFirstAsync(p => p.CustomerId == id);
            if(!result.Succeeded || result.Result == null)
            {
                return AppResult<PayoutAccountDTO>.CreateFailed(new ApplicationException(result.Message), result.Message);
            }

            return AppResult<PayoutAccountDTO>.CreateSucceeded(new PayoutAccountDTO {
                AccountHolder = result.Result.AccountHolder,
                AccountNumber = result.Result.AccountNumber,
                CustomerId = result.Result.CustomerId,
                Id = result.Result.Id,
                Payload = result.Result.Payloads,
                BankChannel = result.Result.BankChannel
            }, "Successfully get payout account by customer id");
        }
        catch (Exception ex)
        {
            return AppResult<PayoutAccountDTO>.CreateFailed(ex, "An error occured when getting payout account by customer id");
        }
    }

    public async Task<AppResult<PayoutAccountDTO>> GetByIdAsync(int id)
    {
        try
        {
            var result = await dataStore.PayoutAccount.GetByIdAsync(id);
            if(!result.Succeeded || result.Result == null)
            {
                return AppResult<PayoutAccountDTO>.CreateFailed(new ApplicationException(result.Message), result.Message);
            }

            return AppResult<PayoutAccountDTO>.CreateSucceeded(new PayoutAccountDTO {
                AccountHolder = result.Result.AccountHolder,
                AccountNumber = result.Result.AccountNumber,
                CustomerId = result.Result.CustomerId,
                Id = result.Result.Id,
                Payload = result.Result.Payloads,
                BankChannel = result.Result.BankChannel
            }, "Successfully get payout account by id");            
        }
        catch (Exception ex)
        {
            return AppResult<PayoutAccountDTO>.CreateFailed(ex, "An error occured when getting payout account by id");
        }
    }

    public async Task<AppResult<PayoutAccountDTO>> Update(int id, string? accountNo, string? accountHolder, string? payload, string? bankChannel)
    {
        try
        {
            var getPayoutAccountRes = await dataStore.PayoutAccount.GetByIdAsync(id);
            if(!getPayoutAccountRes.Succeeded || getPayoutAccountRes.Result == null)
            {
                return AppResult<PayoutAccountDTO>.CreateFailed(new ApplicationException("Invalid payout account id"), "Invalid payout account id");
            }
            var account = getPayoutAccountRes.Result;

            account.AccountHolder = accountHolder ?? account.AccountHolder;
            account.AccountNumber = accountNo ?? account.AccountNumber;
            account.Payloads = payload ?? account.Payloads;
            account.BankChannel = bankChannel ?? account.BankChannel;

            var updatedRes = await dataStore.PayoutAccount.Update(account);
            if(!updatedRes.Succeeded || updatedRes.Result == null)
            {
                return AppResult<PayoutAccountDTO>.CreateFailed(new ApplicationException(updatedRes.Message), updatedRes.Message);
            }
            var updated = updatedRes.Result;

            return AppResult<PayoutAccountDTO>.CreateSucceeded(new PayoutAccountDTO {
                AccountHolder = updated.AccountHolder,
                AccountNumber = updated.AccountNumber,
                CustomerId = updated.CustomerId,
                Id = updated.Id,
                Payload = updated.Payloads,
                BankChannel = updated.BankChannel
            }, "Successfully update payout account");
        }
        catch (Exception ex)
        {
            return AppResult<PayoutAccountDTO>.CreateFailed(ex, "An error occured when updating payout account");
        }
    }
}