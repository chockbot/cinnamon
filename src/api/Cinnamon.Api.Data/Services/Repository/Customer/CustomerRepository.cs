using System.Linq.Expressions;
using Cinnamon.Api.Data.Extensions;
using Cinnamon.Api.Data.Repository.Interfaces;
using Cinnamon.Api.Data.Services.Repository.Interfaces;
using Cinnamon.Framework.ApiCommand.ApiData.DTO.Customer;
using Cinnamon.Framework.Common;
using Microsoft.AspNetCore.Identity;
using Entities = Cinnamon.Api.Data.Repository.Entities;

namespace Cinnamon.Api.Data.Services.Repository.Customer;

public class CustomerRepository : ICustomerRepository
{

    private readonly IDataStore dataStore;
    private readonly UserManager<IdentityUser> userManager;
    private readonly IUserStore<IdentityUser> userStore;
    private readonly IUserEmailStore<IdentityUser> emailStore;

    public CustomerRepository(IDataStore dataStore, UserManager<IdentityUser> userManager,IUserStore<IdentityUser> userStore)
    {
        this.dataStore = dataStore;
        this.userManager = userManager;
        this.userStore = userStore;
        this.emailStore = GetEmailStore();
    }

    public async Task<AppResult<string>> GenerateResetPasswordToken(string email)
    {
        try
        {
            var user = await userManager.FindByEmailAsync(email);
            if(user == null)
            {
                return AppResult<string>.CreateFailed(new ApplicationException("Invalid email provided"), "Invalid email provided");
            }

            var token = await userManager.GeneratePasswordResetTokenAsync(user);
            if(string.IsNullOrEmpty(token))
            {
                return AppResult<string>.CreateFailed(new ApplicationException("An error occured when generate token."), "An error occured when generate token.");
            }

            return AppResult<string>.CreateSucceeded(token, "Successfully generate token for reset password");
        }
        catch (Exception ex)
        {
            return AppResult<string>.CreateFailed(ex, "An error occured when generating reset password token");
        }
    }

    public async Task<AppResult<bool>> ResetPassword(string email, string token, string newPassword)
    {
        try
        {
            var user = await userManager.FindByEmailAsync(email);
            if(user == null)
            {
                return AppResult<bool>.CreateFailed(new ApplicationException("Can't find customer account."), "Can't find customer account.");
            }

            var result = await userManager.ResetPasswordAsync(user, token, newPassword);
            if(!result.Succeeded)
            {
                return AppResult<bool>.CreateFailed(new ApplicationException("An error occured when updating password"), "An error occured when updating password");
            }

            return AppResult<bool>.CreateSucceeded(true, "Successfully reset password");
        }
        catch (Exception ex)
        {
            return AppResult<bool>.CreateFailed(ex, "An error occured when resetting password");
        }
    }

    public async Task<AppResult<CustomerDTO>> CheckLogin(string email, string password)
    {
        try
        {
            var user = await userManager.FindByEmailAsync(email);
            if(user == null)
            {
                return AppResult<CustomerDTO>.CreateFailed(new ApplicationException("Invalid username or password"), "Invalid username or password");
            }

            var checkLogin = await userManager.CheckPasswordAsync(user, password);
            if(!checkLogin)
            {
                return AppResult<CustomerDTO>.CreateFailed(new ApplicationException("Invalid username or password"), "Invalid username or password"); 
            }

            // get customer information
            var customerData = await dataStore.Customer.FindFirstAsync(c => c.UserId == user.Id);
            if(!customerData.Succeeded || customerData.Result == null)
            {
                return AppResult<CustomerDTO>.CreateFailed(
                    new ApplicationException("An error occured when checking login credential"), "An error occured when checking login credential");
            }
            var validCustomer = customerData.Result;

            var customer = new CustomerDTO {
                About = validCustomer.About,
                Birthdate = validCustomer.Birthdate,
                Email = validCustomer.Email,
                ExternalLogin = validCustomer.ExternalLogin,
                FirstName = validCustomer.FirstName,
                Id = validCustomer.Id,
                IsMaker = validCustomer.IsMaker,
                IsVerified = validCustomer.IsVerifiedBadge,
                LastName = validCustomer.LastName,
                ProfileImg = validCustomer.ProfilePath,
                DateJoined = validCustomer.DateJoined,
                Handler = validCustomer.Handler,
                TotalCredits = validCustomer.TotalCredits,
                IsAccountBan = validCustomer.IsAccountBan
            };

            return AppResult<CustomerDTO>.CreateSucceeded(customer, "Success checking login credential");
        }
        catch (Exception ex)
        {
            return AppResult<CustomerDTO>.CreateFailed(ex, "An error occured when checking login credential");
        }
    }

    public async Task<AppResult<CustomerDTO>> Create(string userId, string firstname, string lastname, string email, DateTime birthdate, string phoneNumber,
        string? about, string profilePath, bool ismaker, bool externalLogin, string handler, bool hasAcceptedTerms, bool IsGuest)
    {
        try
        {
            // check userid if existed
            var user = await userManager.FindByIdAsync(userId);
            if(user == null)
            {
                return AppResult<CustomerDTO>.CreateFailed(new ApplicationException("Can't find provided user id"), "Can't find provided user id");
            }

            // set utc for postgres reason
            birthdate = birthdate.SetKindUtc();

            var customer = new Entities.Customer
            {
                About = about,
                ProfilePath = profilePath,
                Email = email,
                Birthdate = birthdate,
                PhoneNumber = phoneNumber,
                ExternalLogin = externalLogin,
                IsMaker = ismaker,
                FirstName = firstname,
                LastName = lastname,
                IsVerifiedBadge = 0,
                UserId = userId,
                Handler = handler,
                HasAcceptedTerms = hasAcceptedTerms,
                IsGuest = IsGuest
            };

            var createdCustomerRes = await dataStore.Customer.Add(customer);
            if(!createdCustomerRes.Succeeded || createdCustomerRes.Result == null)
            {
                return AppResult<CustomerDTO>.CreateFailed(new ApplicationException("An error occured when creating customer"), "An error occured when creating customer");
            }
            var createdCustomer = createdCustomerRes.Result;

            return AppResult<CustomerDTO>.CreateSucceeded(new CustomerDTO
            {
                About = createdCustomer.About,
                Birthdate = createdCustomer.Birthdate,
                PhoneNumber = createdCustomer.PhoneNumber,
                DateJoined = createdCustomer.CreatedOn,
                Email = createdCustomer.Email,
                FirstName = createdCustomer.FirstName,
                LastName = createdCustomer.LastName,
                IsVerified = createdCustomer.IsVerifiedBadge,
                ExternalLogin = createdCustomer.ExternalLogin,
                IsMaker = createdCustomer.IsMaker,
                Id = createdCustomer.Id,
                ProfileImg = createdCustomer.ProfilePath,
                Handler = createdCustomer.Handler,
                IsAccountBan = createdCustomer.IsAccountBan

            }, "Successfully created customer data");
        }
        catch (Exception ex)
        {
            return AppResult<CustomerDTO>.CreateFailed(ex, "An error occured in creating customer");
        }
    }

    public async Task<AppResult<CustomerDTO>> CreateWithPassword(string firstname, string lastname, string email, 
        DateTime birthdate, string phoneNumber, string? about, string profilePath, bool isMaker, bool externalLogin, string pasword, string handler, bool hasAcceptedTerms, bool IsGuest)
    {
        try
        {
            var user = CreateUser();

            // register user using identity framework
            await userStore.SetUserNameAsync(user, email, CancellationToken.None);
            await emailStore.SetEmailAsync(user, email, CancellationToken.None);
            var createUserResult = await userManager.CreateAsync(user, pasword);

            if(!createUserResult.Succeeded)
            {
                string errors = string.Empty;
                foreach(var error in createUserResult.Errors)
                {
                    errors += error.Description + ". ";
                }

                return AppResult<CustomerDTO>.CreateFailed(new ApplicationException(errors), errors);
            }

            var userId = await userManager.GetUserIdAsync(user);

            return await Create(userId, firstname, lastname, email, birthdate, phoneNumber, about, profilePath, isMaker, externalLogin, handler, hasAcceptedTerms, IsGuest);
        }
        catch (Exception ex)
        {
            return AppResult<CustomerDTO>.CreateFailed(ex, "An error occured in getting customers");
        }
    }

public async Task<AppResult<IEnumerable<CustomerDTO>>> GetAllAsync(bool? isVerified, string searchValue, 
    int? count, int? skip, string? handlerLike = null, bool? isOfficialPartner = false, bool? hasVerification = false)
{
    try
    {
        Expression<Func<Entities.Customer, bool>> filter = 
            a => /*(isVerified.HasValue ? a.IsVerifiedBadge == 2 : true) &&*/
                (hasVerification.HasValue ? 
                    (hasVerification.Value ? (a.BackIdImagePath != null && a.FrontIdImagePath != null) : 
                        (a.BackIdImagePath == null && a.FrontIdImagePath == null) ) : 
                    true) &&
                (string.IsNullOrEmpty(handlerLike) ? true : a.Handler.ToLower().Contains(handlerLike.ToLower())) &&
                (isOfficialPartner.HasValue && isOfficialPartner.Value ? a.IsOfficialPartner == true : true);

        var result = await dataStore.Customer.FindCustomerAsync(filter, searchValue, null, null);

        if (!result.Succeeded || result.Result == null)
        {
            return AppResult<IEnumerable<CustomerDTO>>.CreateFailed(result.Error.Exception, result.Message);
        }
        
        var customers = result.Result.Select(c =>
        {
            return new CustomerDTO
            {
                About = c.About,
                Birthdate = c.Birthdate,
                DateJoined = c.CreatedOn,
                PhoneNumber = c.PhoneNumber,
                Email = c.Email,
                ExternalLogin = c.ExternalLogin,
                FirstName = c.FirstName,
                LastName = c.LastName,
                Id = c.Id,
                IsMaker = c.IsMaker,
                IsVerified = c.IsVerifiedBadge,
                IsVerifiedObtainedDate = c.IsVerifiedDate,
                IsOG = c.IsOG,
                IsOGObtainedDate = c.IsOGDate,
                IsOfficial = c.IsOfficialPartner,
                IsOfficialObtainedDate = c.IsOfficialDate,
                ProfileImg = c.ProfilePath,
                Handler = c.Handler,
                BackIdImagePath = c.BackIdImagePath,
                FrontIdImagePath = c.FrontIdImagePath,
                TotalCredits = c.TotalCredits,
                CustomerPricing = new CustomerDTO.Pricing {
                    Rate = c.CustomerPricing != null ? c.CustomerPricing.Rate : 0,
                    IsManualPayment = c.CustomerPricing != null ? c.CustomerPricing.IsManualPayment : false,
                    InclusivePricing = c.CustomerPricing != null ? c.CustomerPricing.InclusivePricing : false
                },
                IsAccountBan = c.IsAccountBan
            };
        });

        var sortedCustomers = customers
            .OrderByDescending(c => c.DateJoined)
            .ThenBy(c => c.IsVerified)
            .ThenBy(c => c.IsVerifiedObtainedDate);

        var paginatedCustomers = sortedCustomers
            .Skip(skip ?? 0)
            .Take(count ?? sortedCustomers.Count());

        return AppResult<IEnumerable<CustomerDTO>>.CreateSucceeded(paginatedCustomers, "Successfully retrieved customers");
    }
    catch (Exception ex)
    {
        return AppResult<IEnumerable<CustomerDTO>>.CreateFailed(ex, "An error occurred in getting customers");
    }
}


    public async Task<AppResult<IEnumerable<CustomerDTO>>> GetAllAsync()
    {
        try
        {
            var result = await dataStore.Customer.GetAllAsync();
            if (!result.Succeeded || result.Result == null)
            {
                return AppResult<IEnumerable<CustomerDTO>>.CreateFailed(result.Error.Exception, result.Message);
            }

            var customers = result.Result.Select(c =>
            {
                return new CustomerDTO
                {
                    About = c.About,
                    Birthdate = c.Birthdate,
                    DateJoined = c.CreatedOn,
                    PhoneNumber = c.PhoneNumber,
                    Email = c.Email,
                    ExternalLogin = c.ExternalLogin,
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    Id = c.Id,
                    IsMaker = c.IsMaker,
                    IsVerified = c.IsVerifiedBadge,
                    IsVerifiedObtainedDate = c.IsVerifiedDate,
                    IsOG = c.IsOG,
                    IsOGObtainedDate = c.IsOGDate,
                    IsOfficial = c.IsOfficialPartner,
                    IsOfficialObtainedDate = c.IsOfficialDate,
                    ProfileImg = c.ProfilePath,
                    Handler =c.Handler,
                    TotalCredits = c.TotalCredits,
                    IsAccountBan = c.IsAccountBan
                };
            });

            return AppResult<IEnumerable<CustomerDTO>>.CreateSucceeded(customers, "Successfully get customers");
        }
        catch (Exception ex)
        {
            return AppResult<IEnumerable<CustomerDTO>>.CreateFailed(ex, "An error occured in getting customers");
        }
    }
    public async Task<AppResult<CustomerDTO>> GetByEmailAsync(string email)
    {
        try
        {
            var result = await dataStore.Customer.GetCustomerByEmail(email);
            if (!result.Succeeded || result.Result == null)
            {
                return AppResult<CustomerDTO>.CreateFailed(result.Error.Exception, result.Message);
            }

            var customerDTO = new CustomerDTO
            {
                About = result.Result.About,
                Birthdate = result.Result.Birthdate,
                DateJoined = result.Result.CreatedOn,
                PhoneNumber = result.Result.PhoneNumber,
                Email = result.Result.Email,
                ExternalLogin = result.Result.ExternalLogin,
                FirstName = result.Result.FirstName,
                LastName = result.Result.LastName,
                Id = result.Result.Id,
                IsMaker = result.Result.IsMaker,
                IsVerified = result.Result.IsVerifiedBadge,
                IsOG = result.Result.IsOG,  
                IsOfficial = result.Result.IsOfficialPartner,
                ProfileImg = result.Result.ProfilePath,
                Handler = result.Result.Handler,
                TotalCredits = result.Result.TotalCredits,
                IsAccountBan = result.Result.IsAccountBan
            };

            return AppResult<CustomerDTO>.CreateSucceeded(customerDTO, "Successfully getting customer by email");
        }
        catch (Exception ex)
        {
            return AppResult<CustomerDTO>.CreateFailed(ex, "An error occured in getting customer by email");
        }
    }

    public async Task<AppResult<CustomerDTO>> GetByIdAsync(int id)
    {
        try
        {
            var result = await dataStore.Customer.GetByIdAsync(id);
            if (!result.Succeeded || result.Result == null)
            {
                return AppResult<CustomerDTO>.CreateFailed(result.Error.Exception, result.Message);
            }

            var customerDTO = new CustomerDTO
            {
                About = result.Result.About,
                Birthdate = result.Result.Birthdate,
                DateJoined = result.Result.CreatedOn,
                Email = result.Result.Email,
                ExternalLogin = result.Result.ExternalLogin,
                FirstName = result.Result.FirstName,
                LastName = result.Result.LastName,
                Id = result.Result.Id,
                IsMaker = result.Result.IsMaker,
                IsVerified = result.Result.IsVerifiedBadge,
                IsVerifiedObtainedDate = result.Result.IsVerifiedDate,
                IsOG = result.Result.IsOG,
                IsOGObtainedDate = result.Result.IsOGDate, 
                IsOfficial = result.Result.IsOfficialPartner,
                IsOfficialObtainedDate = result.Result.IsOfficialDate,
                ProfileImg = result.Result.ProfilePath,
                Handler = result.Result.Handler,
                TotalCredits = result.Result.TotalCredits,
                PhoneNumber = result.Result.PhoneNumber,
                ConnectionId= result.Result.ConnectionId,
                IsAccountBan = result.Result.IsAccountBan
            };

            return AppResult<CustomerDTO>.CreateSucceeded(customerDTO, "Successfully getting customer by id");
        }
        catch (Exception ex)
        {
            return AppResult<CustomerDTO>.CreateFailed(ex, "An error occured in getting customer by id");
        }
    }

    public async Task<AppResult<CustomerDTO>> GetByHandlerAsync(string handler)
    {
        try
        {
            var result = await dataStore.Customer.FindFirstAsync(c => c.Handler == handler);
            if (!result.Succeeded || result.Result == null)
            {
                return AppResult<CustomerDTO>.CreateFailed(result.Error.Exception, result.Message);
            }

            var customerDTO = new CustomerDTO
            {
                About = result.Result.About,
                Birthdate = result.Result.Birthdate,
                DateJoined = result.Result.CreatedOn,
                Email = result.Result.Email,
                ExternalLogin = result.Result.ExternalLogin,
                FirstName = result.Result.FirstName,
                LastName = result.Result.LastName,
                Id = result.Result.Id,
                IsMaker = result.Result.IsMaker,
                IsVerified = result.Result.IsVerifiedBadge,
                IsOG = result.Result.IsOG,
                IsOfficial = result.Result.IsOfficialPartner,
                ProfileImg = result.Result.ProfilePath,
                Handler = result.Result.Handler,
                TotalCredits = result.Result.TotalCredits,
                IsAccountBan = result.Result.IsAccountBan
            };

            return AppResult<CustomerDTO>.CreateSucceeded(customerDTO, "Successfully getting customer by handler");
        }
        catch (Exception ex)
        {
            return AppResult<CustomerDTO>.CreateFailed(ex, "An error occured in getting customer by handler");
        }
    }

    public async Task<AppResult<GovernmentIDsDTO>> GetGovermentId(int customerID)
    {
        try
        {
            var result = await dataStore.Customer.GetByIdAsync(customerID);
            if (!result.Succeeded || result.Result == null)
            {
                return AppResult<GovernmentIDsDTO>.CreateFailed(result.Error.Exception, result.Message);
            }

            return AppResult<GovernmentIDsDTO>.CreateSucceeded(new GovernmentIDsDTO {
                BackIdImagePath = string.IsNullOrEmpty(result.Result.BackIdImagePath) ? string.Empty : result.Result.BackIdImagePath,
                FrontIdImagePath = string.IsNullOrEmpty(result.Result.FrontIdImagePath) ? string.Empty : result.Result.FrontIdImagePath
            }, "Successfully getting customer government id");
        }
        catch (Exception ex)
        {
            return AppResult<GovernmentIDsDTO>.CreateFailed(ex, "An error occured when getting customer government ids");
        }
    }

    public async Task<AppResult<ProfilePictureDTO>> GetProfilePicture(int customerID)
    {
        try
        {
            var result = await dataStore.Customer.GetByIdAsync(customerID);
            if (!result.Succeeded || result.Result == null)
            {
                return AppResult<ProfilePictureDTO>.CreateFailed(result.Error.Exception, result.Message);
            }

            return AppResult<ProfilePictureDTO>.CreateSucceeded(new ProfilePictureDTO
            {
                ProfileImagePath = string.IsNullOrEmpty(result.Result.ProfilePath) ? string.Empty : result.Result.ProfilePath,
            }, "Successfully getting customer government id");
        }
        catch (Exception ex)
        {
            return AppResult<ProfilePictureDTO>.CreateFailed(ex, "An error occured when getting customer profile picture");
        }
    }
    public async Task<AppResult<CustomerDTO>> Update(int customerId, string? firstname, string? lastname, string? email, DateTime? birthdate, string? phoneNumber,
        string? about, string? profilePath, bool? ismaker, bool? externalLogin, int? isVerified,DateTime? isVerifiedDate, string? frontIdImagePath, string? backIdImageParh,
        decimal? totalCredits, bool? isOG, DateTime? isOGDate, bool? isOF, DateTime? isOfficialDate, string? connectionId, bool? isAccountBan, string? handler = null)
    {
        try
        {
            // check first customer if exist
            var customerRes = await dataStore.Customer.GetByIdAsync(customerId);
            if(!customerRes.Succeeded || customerRes.Result == null)
            {
                return AppResult<CustomerDTO>.CreateFailed(new ApplicationException("Can't find customer to update"), "Can't find customer to update");
            }
            var customer = customerRes.Result;

            customer.FirstName = firstname ?? customer.FirstName;
            customer.LastName = lastname ?? customer.LastName;
            customer.Email = email ?? customer.Email;
            customer.Birthdate = birthdate.HasValue ? birthdate.Value.SetKindUtc() : customer.Birthdate.SetKindUtc();
            customer.PhoneNumber = phoneNumber ?? customer.PhoneNumber;
            customer.About = about ?? customer.About;
            customer.ProfilePath = profilePath ?? customer.ProfilePath;
            customer.IsMaker = ismaker ?? customer.IsMaker;
            customer.ExternalLogin = externalLogin ?? customer.ExternalLogin;
            customer.IsVerifiedBadge = isVerified ?? customer.IsVerifiedBadge;
            customer.IsVerifiedDate = isVerifiedDate ?? customer.IsVerifiedDate;
            customer.FrontIdImagePath = frontIdImagePath ?? customer.FrontIdImagePath;
            customer.BackIdImagePath = backIdImageParh ?? customer.BackIdImagePath;
            customer.TotalCredits = totalCredits ?? customer.TotalCredits;
            customer.IsOG = isOG ?? customer.IsOG;
            customer.IsOGDate = isOGDate ?? customer.IsOGDate;
            customer.IsOfficialPartner = isOF ?? customer.IsOfficialPartner;
            customer.IsOfficialDate = isOfficialDate ?? customer.IsOfficialDate;    
            customer.ConnectionId = connectionId ?? customer.ConnectionId;
            customer.Handler = handler ?? customer.Handler;
            customer.IsAccountBan = isAccountBan ?? customer.IsAccountBan;

            var updatedCustomerRes = await dataStore.Customer.Update(customer);
            if (!updatedCustomerRes.Succeeded)
            {
                return AppResult<CustomerDTO>.CreateFailed(updatedCustomerRes.Error.Exception, updatedCustomerRes.Message);
            }

            return AppResult<CustomerDTO>.CreateSucceeded(new CustomerDTO
            {
                About = customer.About,
                Birthdate = customer.Birthdate,
                DateJoined = customer.CreatedOn,
                Email = customer.Email,
                PhoneNumber = customer.PhoneNumber,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                ExternalLogin = customer.ExternalLogin,
                Id = customer.Id,
                IsMaker = customer.IsMaker,
                IsVerified = customer.IsVerifiedBadge,
                IsVerifiedObtainedDate = customer.IsVerifiedDate,
                ProfileImg = customer.ProfilePath,
                Handler = customer.Handler,
                TotalCredits = customer.TotalCredits,
                IsOG = customer.IsOG,
                IsOGObtainedDate = customer.IsOGDate,
                IsOfficial = customer.IsOfficialPartner,
                IsOfficialObtainedDate = customer.IsOGDate,
                IsAccountBan = customer.IsAccountBan
                
            }, "Successfully updated customer data");
        }
        catch (Exception ex)
        {
            return AppResult<CustomerDTO>.CreateFailed(ex, "An error occured in updating customer data");
        }
    }

    private IdentityUser CreateUser()
    {
        try
        {
            return Activator.CreateInstance<IdentityUser>();
        }
        catch
        {
            throw new InvalidOperationException($"Can't create an instance of '{nameof(IdentityUser)}'. " +
                $"Ensure that '{nameof(IdentityUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
        }
    }

    private IUserEmailStore<IdentityUser> GetEmailStore()
    {
        if (!userManager.SupportsUserEmail)
        {
            throw new NotSupportedException("The default UI requires a user store with email support.");
        }
        return (IUserEmailStore<IdentityUser>)userStore;
    }
    public async Task<AppResult<bool>> ChangeEmailAddress(string currentEmail, string newEmail)
    {
        try
        {
            var user = await userManager.FindByEmailAsync(currentEmail);
            if (user == null)
            {
                return AppResult<bool>.CreateFailed(new ApplicationException("Can't find user account."), "Can't find user account.");
            }
            //var token = await userManager.GenerateChangeEmailTokenAsync(user, newEmail);
            user.UserName = newEmail;
            user.Email    = newEmail;

            var result = await userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                return AppResult<bool>.CreateFailed(new ApplicationException("An error occurred when updating email"), "An error occurred when updating email");
            }

            return AppResult<bool>.CreateSucceeded(true, "Successfully change email");
        }
        catch (Exception ex)
        {
            return AppResult<bool>.CreateFailed(ex, "An error occurred when resetting email");
        }
    }

    public async Task<AppResult<CustomerDTO>> CreateGuestCustomer(string? firstname, string? lastname, string email, DateTime? birthdate, 
        string? phoneNumber, string? about, string? profilePath, string? handler, bool? hasAcceptedTerms)
    {
        try
        {
            // Check if guest customer email already exists
            var existingCustomer = await dataStore.Customer.FindFirstAsync(c => c.Email == email);
            if (existingCustomer.Succeeded && existingCustomer.Result is not null)
            {
                return AppResult<CustomerDTO>.CreateFailed(
                    new ApplicationException("Email already exists"), 
                    "A customer with this email already registered");
            }

            // Set UTC for postgres reason
            birthdate = !birthdate.HasValue ? DateTime.Now.SetKindUtc() : birthdate.Value.SetKindUtc();

            var customer = new Entities.Customer
            {
                About = about,
                ProfilePath = profilePath,
                Email = email,
                Birthdate = birthdate.Value,
                PhoneNumber = phoneNumber,
                ExternalLogin = false,
                IsMaker = false,
                FirstName = firstname,
                LastName = lastname,
                IsVerifiedBadge = 0,
                UserId = null, // Guest customers don't have UserId
                Handler = handler ?? string.Empty,
                HasAcceptedTerms = hasAcceptedTerms ?? false,
                IsGuest = true // Always true for guest customers
            };

            var createdCustomerRes = await dataStore.Customer.Add(customer);
            if (!createdCustomerRes.Succeeded || createdCustomerRes.Result == null)
            {
                return AppResult<CustomerDTO>.CreateFailed(
                    new ApplicationException("An error occurred when creating guest customer"), 
                    "An error occurred when creating guest customer");
            }

            var createdCustomer = createdCustomerRes.Result;

            return AppResult<CustomerDTO>.CreateSucceeded(new CustomerDTO
            {
                About = createdCustomer.About,
                Birthdate = createdCustomer.Birthdate,
                PhoneNumber = createdCustomer.PhoneNumber,
                DateJoined = createdCustomer.CreatedOn,
                Email = createdCustomer.Email,
                FirstName = createdCustomer.FirstName,
                LastName = createdCustomer.LastName,
                IsVerified = createdCustomer.IsVerifiedBadge,
                ExternalLogin = createdCustomer.ExternalLogin,
                IsMaker = createdCustomer.IsMaker,
                Id = createdCustomer.Id,
                ProfileImg = createdCustomer.ProfilePath,
                Handler = createdCustomer.Handler,
                IsAccountBan = createdCustomer.IsAccountBan
            }, "Successfully created guest customer data");
        }
        catch (Exception ex)
        {
            return AppResult<CustomerDTO>.CreateFailed(ex, "An error occurred in creating guest customer");
        }
    }

}