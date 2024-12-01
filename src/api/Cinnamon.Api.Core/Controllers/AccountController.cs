using Microsoft.AspNetCore.Mvc;
using Cinnamon.Api.Core.Services.AccountService.Handlers;
using Cinnamon.Framework.ApiCommand.ApiCore.Account.Request;
using Cinnamon.Framework.ApiCommand.ApiCore.Account.Response;
using Cinnamon.Framework.ApiCommand.ApiCore;
using Cinnamon.Framework.ApiCommand.ApiCore.DTO.Customer;
using Cinnamon.Framework.ApiCommand.ApiCore.DTO.Waitlist;
using Microsoft.AspNetCore.Authorization;
using Cinnamon.Framework.ApiCommand.ApiCore.DTO.FamilyMember;
using Cinnamon.Framework.ApiCommand.ApiCore.DTO.RequestRefund;
using Cinnamon.Framework.ApiCommand.ApiCore.DTO.OTP;
using Cinnamon.Framework.ApiCommand.ApiCore.Activity.Response;

namespace Cinnamon.Api.Core.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class AccountController : ControllerBase 
{

    #region dependencies declaration

    private readonly ISubmitRegisterHandler submitRegisterHandler;
    private readonly ISubmitWaitlistHandler submitWaitlistHandler;
    private readonly ISubmitVerifyEmailHandler submitVerifyEmailHandler;
    private readonly ISubmitLoginHandler submitLoginHandler;
    private readonly ISubmitResendVerificationHandler submitResendEmailHandler;
    private readonly IGetProfileHandler getProfileHandler;
    private readonly IGetFamilyMembersHandler getFamilyMembersHandler;
    private readonly IGetWaitListHandler getWaitListHandler;
    private readonly IUpdateFamilyMembersHandler updateFamilyMembersHandler;
    private readonly ICreateFamilyMembersHandler createFamilyMembersHandler;
    private readonly IDeleteFamilyMembersHandler deleteFamilyMembersHandler;
    private readonly ISubmitUpdateProfileHandler updateProfileHandler;
    private readonly IGetGovernmentIdsHandler getGovernmentIdsHandler;
    private readonly IGetProfilePictureHandler getProfilePictureHandler;
    private readonly IUploadGovernmentIdHandler uploadGovernmentIdHandler;
    private readonly IUploadProfilePictureHandler uploadProfilePictureHandler;
    private readonly IGetCustomerByEmailHandler getCustomerByEmailHandler;
    private readonly IGetCustomerByIdHandler getCustomerByIdHandler;
    private readonly IGetWaitListByGuidHandler getWaitListByGuidHandler;
    private readonly IExternalLoginHandler externalLoginHandler;
    private readonly IExternalRegisterHandler externalRegisterHandler;
    private readonly IGetExternalLoginDetailHandler getExternalLoginDetailHandler;
    private readonly IGetCustomerByHandler getCustomerByHandler;
    private readonly IResetPasswordHandler resetPasswordHandler;
    private readonly IVerifyResetPasswordHandler verifyResetPasswordHandler;
    private readonly IRequestRefundHandler requestRefundHandler;
    private readonly IGetRequestRefundHandler getRequestRefundHandler;
    private readonly IDeleteProfilePictureHandler deleteProfilePictureHandler;
    private readonly IGetPayoutAccountHandler getPayoutAccountHandler;
    private readonly ICreateUpdatePayoutAccountHandler createUpdatePayoutAccountHandler;
    private readonly IGetAllCustomersHandler getAllCustomersHandler;
    private readonly IUpdateCustomerProfileHandler updateCustomerProfileHandler;
    private readonly IUpdateRequestRefundHandler updateRequestRefundHandler;
    private readonly IAccountSubmitVerifiedHandler accountSubmitVerifiedHandler;
    private readonly IUpdateConnectionIdHandler updateConnectionIdHandler;
    private readonly IVerifyUserNotificationHandler verifyUserNotificationHandler;
    private readonly IBlockedAccountHandler blockedAccountHandler;
    private readonly IExtraLoginHandler extraLoginHandler;
    private readonly IChangeEmailHandler changeEmailHandler;
    private readonly IDeleteWaitlistHandler deleteWaitlistHandler;
    private readonly ISendOTPHandler sendOTPHandler;
    private readonly IGetUserOTPHandler getUserOTPHandler;
    private readonly IVerifyOTPHandler verifyOTPHandler;
    private readonly ICreateGuestCustomerHandler createGuestCustomerHandler;

    #endregion

    #region constructor

    public AccountController(ISubmitRegisterHandler submitRegisterHandler, ISubmitWaitlistHandler submitWaitlistHandler,
        ISubmitVerifyEmailHandler submitVerifyEmailHandler, ISubmitLoginHandler submitLoginHandler,
        ISubmitResendVerificationHandler submitResendEmailHandler, IGetProfileHandler getProfileHandler,
        IGetFamilyMembersHandler getFamilyMembersHandler, IUpdateFamilyMembersHandler updateFamilyMembersHandler,
        ICreateFamilyMembersHandler createFamilyMembersHandler, IDeleteFamilyMembersHandler deleteFamilyMembersHandler,
        ISubmitUpdateProfileHandler updateProfileHandler, IGetGovernmentIdsHandler getGovernmentIdsHandler,
        IUploadGovernmentIdHandler uploadGovernmentIdHandler, IUploadProfilePictureHandler uploadProfilePictureHandler, IGetProfilePictureHandler getProfilePictureHandler,
        IGetWaitListHandler getWaitListHandler, IGetCustomerByEmailHandler getCustomerByEmailHandler, IGetWaitListByGuidHandler getWaitListByGuidHandler,
        IGetCustomerByIdHandler getCustomerByIdHandler, IExternalLoginHandler externalLoginHandler, IExternalRegisterHandler externalRegisterHandler,
        IGetExternalLoginDetailHandler getExternalLoginDetailHandler, IGetCustomerByHandler getCustomerByHandler,
        IResetPasswordHandler resetPasswordHandler, IVerifyResetPasswordHandler verifyResetPasswordHandler,
        IRequestRefundHandler requestRefundHandler, IGetRequestRefundHandler getRequestRefundHandler, IDeleteProfilePictureHandler deleteProfilePictureHandler,
        IGetPayoutAccountHandler getPayoutAccountHandler, ICreateUpdatePayoutAccountHandler createUpdatePayoutAccountHandler,
        IGetAllCustomersHandler getAllCustomersHandler, IUpdateCustomerProfileHandler updateCustomerProfileHandler,
        IUpdateRequestRefundHandler updateRequestRefundHandler, IAccountSubmitVerifiedHandler accountSubmitVerifiedHandler, 
        IUpdateConnectionIdHandler updateConnectionIdHandler, IVerifyUserNotificationHandler verifyUserNotificationHandler,
        IBlockedAccountHandler blockedAccountHandler, IExtraLoginHandler extraLoginHandler, IChangeEmailHandler changeEmailHandler, IDeleteWaitlistHandler deleteWaitlistHandler,
        ISendOTPHandler sendOTPHandler, IGetUserOTPHandler getUserOTPHandler,IVerifyOTPHandler verifyOTPHandler,
        ICreateGuestCustomerHandler createGuestCustomerHandler)
    {
        this.submitRegisterHandler            = submitRegisterHandler;
        this.submitWaitlistHandler            = submitWaitlistHandler;
        this.submitVerifyEmailHandler         = submitVerifyEmailHandler;
        this.submitLoginHandler               = submitLoginHandler;
        this.submitResendEmailHandler         = submitResendEmailHandler;
        this.getProfileHandler                = getProfileHandler;
        this.getFamilyMembersHandler          = getFamilyMembersHandler;
        this.updateFamilyMembersHandler       = updateFamilyMembersHandler;
        this.createFamilyMembersHandler       = createFamilyMembersHandler;
        this.deleteFamilyMembersHandler       = deleteFamilyMembersHandler;
        this.updateProfileHandler             = updateProfileHandler;
        this.getGovernmentIdsHandler          = getGovernmentIdsHandler;
        this.uploadGovernmentIdHandler        = uploadGovernmentIdHandler;
        this.uploadProfilePictureHandler      = uploadProfilePictureHandler;
        this.getProfilePictureHandler         = getProfilePictureHandler;
        this.getWaitListHandler               = getWaitListHandler;
        this.getCustomerByEmailHandler        = getCustomerByEmailHandler;
        this.getWaitListByGuidHandler         = getWaitListByGuidHandler;
        this.getCustomerByIdHandler           = getCustomerByIdHandler;
        this.externalLoginHandler             = externalLoginHandler;
        this.externalRegisterHandler          = externalRegisterHandler;
        this.getExternalLoginDetailHandler    = getExternalLoginDetailHandler;
        this.getCustomerByHandler             = getCustomerByHandler;
        this.resetPasswordHandler             = resetPasswordHandler;
        this.verifyResetPasswordHandler       = verifyResetPasswordHandler;
        this.requestRefundHandler             = requestRefundHandler;
        this.getRequestRefundHandler          = getRequestRefundHandler;
        this.deleteProfilePictureHandler      = deleteProfilePictureHandler;
        this.getPayoutAccountHandler          = getPayoutAccountHandler;
        this.createUpdatePayoutAccountHandler = createUpdatePayoutAccountHandler;
        this.getAllCustomersHandler           = getAllCustomersHandler;
        this.updateCustomerProfileHandler     = updateCustomerProfileHandler;
        this.updateRequestRefundHandler       = updateRequestRefundHandler;
        this.accountSubmitVerifiedHandler     = accountSubmitVerifiedHandler;
        this.updateConnectionIdHandler        = updateConnectionIdHandler;
        this.verifyUserNotificationHandler    = verifyUserNotificationHandler;
        this.blockedAccountHandler            = blockedAccountHandler;
        this.extraLoginHandler                = extraLoginHandler;
        this.changeEmailHandler               = changeEmailHandler;   
        this.deleteWaitlistHandler            = deleteWaitlistHandler;
        this.sendOTPHandler                   = sendOTPHandler;
        this.getUserOTPHandler                = getUserOTPHandler;
        this.verifyOTPHandler                 = verifyOTPHandler;
        this.createGuestCustomerHandler       = createGuestCustomerHandler;
    }

    #endregion

    [Route("Register")]
    [HttpPost]
    [ProducesResponseType(typeof(SubmitRegisterResult), StatusCodes.Status201Created)]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] SubmitRegisterArgs args)
    {
        try
        {
            var result = await submitRegisterHandler.ExecuteAsync(new Services.AccountService.Interactors.SubmitRegisterArgs {
                Birthdate = args.Birthdate,
                Email = args.Email,
                ExternalLogin = args.ExternalLogin,
                PhoneNumber = args.PhoneNumber,
                FirstName = args.FirstName,
                LastName = args.LastName,
                Password = args.Password,
                ProfilePath = args.ProfilePath,
                HasAcceptedTerms = args.HasAcceptedTerms,
                IsGuest = args.IsGuest
            });

            if(!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new SubmitRegisterResult {ErrorInfo = new ErrorInfo {Message = result.Message}});
            }
            var objResult = result.Result;

            return new JsonResult(new SubmitRegisterResult {
                Result = new CustomerDTO {
                    Birthdate = objResult.Birthdate,
                    Email = objResult.Email,
                    PhoneNumber = objResult.PhoneNumber,
                    ExternalLogin = objResult.ExternalLogin,
                    FirstName = objResult.FirstName,
                    LastName = objResult.LastName,
                    ProfileImg = objResult.ProfileImg,
                    Id = objResult.Id,
                    Handler = objResult.Handler,
                },
                IsSuccess = true,
            });
        }
        catch (Exception ex)
        {
            return new JsonResult(new SubmitRegisterResult {ErrorInfo = new ErrorInfo {Message = ex.Message}});
        }
    }

    [Route("ExternalRegister")]
    [HttpPost]
    [ProducesResponseType(typeof(SubmitRegisterResult), StatusCodes.Status201Created)]
    [AllowAnonymous]
    public async Task<IActionResult> ExternalRegister([FromBody] SubmitExternalRegisterArgs args)
    {
        try
        {
            var result = await externalRegisterHandler.ExecuteAsync(new Services.AccountService.Interactors.ExternalRegisterArgs {
                Birthdate = args.Birthdate,
                Email = args.Email,
                FirstName = args.FirstName,
                PhoneNumber = args.PhoneNumber,
                Guid = args.Guid,
                LastName = args.LastName,
                Password = args.Password,
                ProfilePath = args.ProfilePath,
                Token = args.Token,
                HasAcceptedTerms = args.HasAcceptedTerms
            });

            if(!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new SubmitRegisterResult {ErrorInfo = new ErrorInfo {Message = result.Message}});
            }
            var objResult = result.Result;

            return new JsonResult(new SubmitRegisterResult {
                Result = new CustomerDTO {
                    Birthdate = objResult.Birthdate,
                    Email = objResult.Email,
                    PhoneNumber = objResult.PhoneNumber,
                    ExternalLogin = objResult.ExternalLogin,
                    FirstName = objResult.FirstName,
                    LastName = objResult.LastName,
                    ProfileImg = objResult.ProfileImg,
                    Id = objResult.Id,
                    Handler = objResult.Handler
                },
                IsSuccess = true,
            });
        }
        catch (Exception ex)
        {
            return new JsonResult(new SubmitRegisterResult {ErrorInfo = new ErrorInfo {Message = ex.Message}});
        }
    }

    [Route("RegisterWaitlist")]
    [HttpPost]
    [ProducesResponseType(typeof(RegisterWaitlistResult), StatusCodes.Status201Created)]
    [AllowAnonymous]
    public async Task<IActionResult> RegisterWaitlist([FromBody] RegisterWaitlistArgs args)
    {
        try
        {
            var result = await submitWaitlistHandler.ExecuteAsync(new Services.AccountService.Interactors.SubmitWaitlistArgs{
                Email = args.Email,
                ValidationRoute = args.ValidationRoute,
                IsGuest = args.IsGuest
            });

            if(!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new RegisterWaitlistResult {ErrorInfo = new ErrorInfo {Message = result.Message, Code = result.Error.Code}});
            }
            var created = result.Result;

            return new JsonResult(new RegisterWaitlistResult {
                Result = new VerificationLinkDTO {
                    Email = created.Email,
                    Guid = created.Guid,
                    Token = created.Token,
                    Id = created.Id,
                    VerificationLink = created.VerificationLink
                },
                IsSuccess = true
            });
        }
        catch (Exception ex)
        {
            return new JsonResult(new RegisterWaitlistResult {ErrorInfo = new ErrorInfo {Message = ex.Message}});
        }
    }

    [Route("VerifyRegisteredEmail")]
    [HttpPost]
    [ProducesResponseType(typeof(VerifyRegisteredEmailResult), StatusCodes.Status201Created)]
    [AllowAnonymous]
    public async Task<IActionResult> VerifyRegisteredEmail([FromBody] VerifyRegisteredEmailArgs args)
    {
        try
        {
            var result = await submitVerifyEmailHandler.ExecuteAsync(new Services.AccountService.Interactors.SubmitVerifyEmailArgs {
                Token = args.Token,
                UserId = args.UserId
            });

            if(!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new VerifyRegisteredEmailResult {ErrorInfo = new ErrorInfo {Message = result.Message}});
            }
            var verified = result.Result;

            return new JsonResult(new VerifyRegisteredEmailResult {
                Result = new WaitlistDTO {
                    Email = verified.Email,
                    Guid = verified.Guid,
                    Token = verified.Token,
                    Id = verified.Id,
                    IsVerified = verified.IsVerified
                },
                IsSuccess = true
            });
        }
        catch (Exception ex)
        {
            return new JsonResult(new VerifyRegisteredEmailResult {ErrorInfo = new ErrorInfo {Message = ex.Message}});
        }
    }

    [Route("ResendVerification")]
    [HttpPost]
    [ProducesResponseType(typeof(ResendVerificationResult), StatusCodes.Status201Created)]
    [AllowAnonymous]
    public async Task<IActionResult> ResendVerification([FromBody] ResendVerificationArgs args)
    {
        try
        {
            var result = await submitResendEmailHandler.ExecuteAsync(new Services.AccountService.Interactors.SubmitResendVerificationArgs {
                Email = args.Email,
                ValidationRoute = args.ValidationRoute
            });

            if(!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new ResendVerificationResult {ErrorInfo = new ErrorInfo {Message = result.Message}});
            }
            var verified = result.Result;

            return new JsonResult(new ResendVerificationResult {
                Result = new WaitlistDTO {
                    Email = verified.Email,
                    Guid = verified.Guid,
                    Token = verified.Token,
                    Id = verified.Id,
                },
                IsSuccess = true
            });
        }
        catch (Exception ex)
        {
            return new JsonResult(new ResendVerificationResult {ErrorInfo = new ErrorInfo {Message = ex.Message}});
        }
    }

    [Route("Login")]
    [HttpPost]
    [ProducesResponseType(typeof(VerifiedLoginResult), StatusCodes.Status201Created)]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] VerifiedLoginArgs args)
    {
        try
        {
            var result = await submitLoginHandler.ExecuteAsync(new Services.AccountService.Interactors.SubmitLoginArgs {
                Email = args.Email,
                Password = args.Password
            });

            if(!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new VerifiedLoginResult {ErrorInfo = new ErrorInfo {Message = result.Message}});
            }
            var verified = result.Result;

            return new JsonResult(new VerifiedLoginResult {
                Result = new VerifiedLoginDTO {
                    Email = verified.Email,
                    ExternalLogin = verified.ExternalLogin,
                    FirstName = verified.FirstName,
                    LastName = verified.LastName,
                    IsMaker = verified.IsMaker,
                    Id = verified.Id,
                    GeneratedToken = verified.GeneratedToken
                },
                IsSuccess = true
            });
        }
        catch (Exception ex)
        {
            return new JsonResult(new VerifiedLoginResult {ErrorInfo = new ErrorInfo {Message = ex.Message}});
        }
    }

    [Route("GetProfile")]
    [HttpGet]
    [ProducesResponseType(typeof(GetProfileResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetProfile([FromQuery] GetProfileArgs args)
    {
        try
        {
            var result = await getProfileHandler.ExecuteAsync(new Services.AccountService.Interactors.GetProfileArgs {});

            if(!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new GetProfileResult {ErrorInfo = new ErrorInfo {Message = result.Message}});
            }
            var profile = result.Result;

            return new JsonResult(new GetProfileResult {
                Result = new ProfileDTO {
                    Email          = profile.Email,
                    FirstName      = profile.FirstName,
                    LastName       = profile.LastName,
                    IsMaker        = profile.IsMaker,
                    Id             = profile.Id,
                    About          = profile.About,
                    Birthdate      = profile.Birthdate,
                    PhoneNumber    = profile.PhoneNumber,
                    DateJoined     = profile.DateJoined,
                    IsVerified     = profile.IsVerified,
                    IsVerifiedDate = profile.IsVerifiedDate,
                    IsOfficial     = profile.IsOfficial,
                    IsOfficialDate = profile.IsOfficialDate,
                    IsOG           = profile.IsOG,
                    IsOGDate       = profile.IsOGDate,
                    ProfileImg     = profile.ProfileImagePath,
                    Handler        = profile.Handler,
                    TotalCredits   = profile.TotalCredits,
                    ConnectionId   = profile.ConnectionId,
                    IsGuest        = profile.IsGuest
                },
                IsSuccess = true
            });
        }
        catch (Exception ex)
        {
            return new JsonResult(new GetProfileResult {ErrorInfo = new ErrorInfo {Message = ex.Message}});
        }
    }

    [Route("GetFamilyMembers")]
    [HttpGet]
    [ProducesResponseType(typeof(GetFamilyMemberResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetFamilyMembers()
    {
        try
        {
            var result = await getFamilyMembersHandler.ExecuteAsync(new Services.AccountService.Interactors.GetFamilyMembersArgs {});

            if(!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new GetFamilyMemberResult {ErrorInfo = new ErrorInfo {Message = result.Message}});
            }
            var members = result.Result.FamilyMembers.Select(i => {
                return new FamilyMemberDTO {
                    BirthMonth = i.BirthMonth,
                    BirthYear = i.BirthYear,
                    Gender = i.Gender,
                    Id = i.Id,
                    Name = i.Name
                };
            });

            return new JsonResult(new GetFamilyMemberResult {
                Result = members,
                IsSuccess = true
            });
        }
        catch (Exception ex)
        {
            return new JsonResult(new GetFamilyMemberResult {ErrorInfo = new ErrorInfo {Message = ex.Message}});
        }
    }

    [Route("GetAllWaitList")]
    [HttpGet]
    [ProducesResponseType(typeof(GetWaitListResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllWaitList()
    {
        try
        {
            var result = await getWaitListHandler.ExecuteAsync(new Services.AccountService.Interactors.GetWaitListArgs { });

            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new GetWaitListResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }
            var waitlist = result.Result.WaitLists.Select(i => {
                return new WaitlistDTO
                {
                    Id= i.Id,   
                    Email= i.Email, 
                    Guid= i.Guid,   
                    IsVerified= i.IsVerified,   
                    Token = i.Token 
                };
            });

            return new JsonResult(new GetWaitListResult
            {
                Result = waitlist,
                IsSuccess = true
            });
        }
        catch (Exception ex)
        {
            return new JsonResult(new GetWaitListResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("UpdateFamilyMembers")]
    [HttpPost]
    [ProducesResponseType(typeof(UpdateFamilyMemberResult), StatusCodes.Status202Accepted)]
    public async Task<IActionResult> UpdateFamilyMembers([FromBody] UpdateFamilyMemberArgs args)
    {
        try
        {
            var result = await updateFamilyMembersHandler.ExecuteAsync(new Services.AccountService.Interactors.UpdateFamilyMembersArgs {
                FamilyMembers = args.FamilyMembers.Select(f => {
                    return new Services.AccountService.Interactors.UpdateFamilyMembersArgs.FamilyMember {
                        BirthMonth = f.BirthMonth,
                        BirthYear = f.BirthYear,
                        Gender = f.Gender,
                        Id = f.Id,
                        Name = f.Name
                    };
                })
            });

            if(!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new UpdateFamilyMemberResult {ErrorInfo = new ErrorInfo {Message = result.Message}});
            }
            var updated = result.Result.FamilyMembers.Select(i => {
                return new FamilyMemberDTO {
                    BirthMonth = i.BirthMonth,
                    BirthYear = i.BirthYear,
                    Gender = i.Gender,
                    Id = i.Id,
                    Name = i.Name
                };
            });

            return new JsonResult(new UpdateFamilyMemberResult {
                Result = updated,
                IsSuccess = true
            });
        }
        catch (Exception ex)
        {
            return new JsonResult(new UpdateFamilyMemberResult {ErrorInfo = new ErrorInfo {Message = ex.Message}});
        }
    }

    [Route("CreateFamilyMembers")]
    [HttpPost]
    [ProducesResponseType(typeof(CreateFamilyMemberResult), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateFamilyMembers([FromBody] CreateFamilyMemberArgs args)
    {
        try
        {
            var result = await createFamilyMembersHandler.ExecuteAsync(new Services.AccountService.Interactors.CreateFamilyMembersArgs {
                FamilyMembers = args.FamilyMembers.Select(f => {
                    return new Services.AccountService.Interactors.CreateFamilyMembersArgs.FamilyMember {
                        BirthMonth = f.BirthMonth,
                        BirthYear = f.BirthYear,
                        Gender = f.Gender,
                        Name = f.Name
                    };
                })
            });

            if(!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new CreateFamilyMemberResult {ErrorInfo = new ErrorInfo {Message = result.Message}});
            }
            var created = result.Result.FamilyMembers.Select(i => {
                return new FamilyMemberDTO {
                    BirthMonth = i.BirthMonth,
                    BirthYear = i.BirthYear,
                    Gender = i.Gender,
                    Id = i.Id,
                    Name = i.Name,
                    CustomerId = i.CustomerId
                };
            });

            return new JsonResult(new CreateFamilyMemberResult {
                Result = created,
                IsSuccess = true
            });
        }
        catch (Exception ex)
        {
            return new JsonResult(new CreateFamilyMemberResult {ErrorInfo = new ErrorInfo {Message = ex.Message}});
        }
    }

    [Route("DeleteFamilyMembers")]
    [HttpPost]
    [ProducesResponseType(typeof(DeleteFamilyMembersResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteFamilyMembers([FromBody] DeleteFamilyMembersArgs args)
    {
        try
        {
            var result = await deleteFamilyMembersHandler.ExecuteAsync(new Services.AccountService.Interactors.DeleteFamilyMembersArgs {
                Ids = args.Ids
            });

            if(!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new DeleteFamilyMembersResult {ErrorInfo = new ErrorInfo {Message = result.Message}});
            }

            return new JsonResult(new DeleteFamilyMembersResult {
                Result = true,
                IsSuccess = true
            });
        }
        catch (Exception ex)
        {
            return new JsonResult(new DeleteFamilyMembersResult {ErrorInfo = new ErrorInfo {Message = ex.Message}});
        }
    }

    [Route("UpdateProfileDetails")]
    [HttpPost]
    [ProducesResponseType(typeof(UpdateProfileDetailsResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateProfileDetails([FromBody] UpdateProfileDetailsArgs args)
    {
        try
        {
            var result = await updateProfileHandler.ExecuteAsync(new Services.AccountService.Interactors.SubmitUpdateProfileArgs {
                About         = args.About,
                Birthdate     = args.Datebirth,
                FirstName     = args.FirstName,
                LastName      = args.LastName,
                Email         = args.Email,
                VerifiedBadge = args.VerifiedBadge,
                PhoneNumber   = args.PhoneNumber
            });

            if(!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new UpdateProfileDetailsResult {ErrorInfo = new ErrorInfo {Message = result.Message}});
            }

            return new JsonResult(new UpdateProfileDetailsResult {
                Result = new CustomerDTO {
                    About       = result.Result.About,
                    Birthdate   = result.Result.Birthdate,
                    FirstName   = result.Result.FirstName,
                    LastName    = result.Result.LastName,
                    Email       = result.Result.Email,
                    PhoneNumber = result.Result.PhoneNumber
                },
                IsSuccess = true
            });
        }
        catch (Exception ex)
        {
            return new JsonResult(new UpdateProfileDetailsResult {ErrorInfo = new ErrorInfo {Message = ex.Message}});
        }
    }

    [Route("GetGovermentIds")]
    [HttpGet]
    [ProducesResponseType(typeof(GetGovernmentIdsResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetGovermentIds()
    {
        try
        {
            var result = await getGovernmentIdsHandler.ExecuteAsync(new Services.AccountService.Interactors.GetGovernmentIdsArgs {});

            if(!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new GetGovernmentIdsResult {ErrorInfo = new ErrorInfo {Message = result.Message}});
            }

            return new JsonResult(new GetGovernmentIdsResult {
                Result = new GovernmentIdsDTO {
                    BackIdImagePath = result.Result.BackImageSrc,
                    FrontIdImagePath = result.Result.FrontImageSrc
                },
                IsSuccess = true
            });
        }
        catch (Exception ex)
        {
            return new JsonResult(new GetGovernmentIdsResult {ErrorInfo = new ErrorInfo {Message = ex.Message}});
        }
    }

    [Route("GetProfilePicture")]
    [HttpGet]
    [ProducesResponseType(typeof(GetProfilePictureResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetProfilePicture()
    {
        try
        {
            var result= await getProfilePictureHandler.ExecuteAsync(new Services.AccountService.Interactors.GetProfilePictureArgs {});
            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new GetProfilePictureResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }
            return new JsonResult(new GetProfilePictureResult
            {
                Result = new ProfilePictureDTO
                {
                    ProfileImagePath = result.Result.ProfileImagseSrc
                },
                IsSuccess = true
            });
        }
        catch (Exception ex)
        {
            return new JsonResult(new GetProfilePictureResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("UploadGovernmentIds")]
    [HttpPost]
    [ProducesResponseType(typeof(UploadGovernmentIdsResult), StatusCodes.Status201Created)]
    public async Task<IActionResult> UploadGovernmentIds([FromForm] UploadGovernmentIdsArgs args)
    {
        try
        {
            var result = await uploadGovernmentIdHandler.ExecuteAsync(new Services.AccountService.Interactors.UploadGovernmentIDArgs {
                BackImage = args.BackImageId,
                FrontImage = args.FrontImageId
            });

            if(!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new UploadGovernmentIdsResult {ErrorInfo = new ErrorInfo {Message = result.Message}});
            }

            return new JsonResult(new UploadGovernmentIdsResult {
                Result = new GovernmentIdsDTO {
                    BackIdImagePath = result.Result.BackImage.UploadedPath,
                    FrontIdImagePath = result.Result.FrontImage.UploadedPath
                },
                IsSuccess = true
            });
        }
        catch (Exception ex)
        {
            return new JsonResult(new UploadGovernmentIdsResult {ErrorInfo = new ErrorInfo {Message = ex.Message}});
        }
    }

    [Route("UploadProfilePicture")]
    [HttpPost]
    [ProducesResponseType(typeof(UploadProfilePictureResult), StatusCodes.Status201Created)]
    public async Task<IActionResult> UploadProfilePicture([FromForm] UploadProfilePictureArgs args)
    {
        try
        {
            var result = await uploadProfilePictureHandler.ExecuteAsync(new Services.AccountService.Interactors.UploadProfilePictureArgs
            {
                ProfileImage = args.ProfileImageId,
            });

            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new UploadGovernmentIdsResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new UploadProfilePictureResult
            {
                Result = new ProfilePictureDTO
                {
                    ProfileImagePath = result.Result.ProfileImage.UploadedPath,
                },
                IsSuccess = true
            });
        }
        catch (Exception ex)
        {
            return new JsonResult(new UploadProfilePictureResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("GetCustomerById/{id}")]
    [HttpGet]
    [ProducesResponseType(typeof(GetCustomerByIdResult), StatusCodes.Status201Created)]
    [AllowAnonymous]
    public async Task<IActionResult> GetMakerDetail(int id)
    {
        try
        {
            var result = await getCustomerByIdHandler.ExecuteAsync(new Services.AccountService.Interactors.GetCustomerByIdArgs{
                Id = id
            });

            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new GetCustomerByIdResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }
            var objResult = result.Result;

            return new JsonResult(new GetCustomerByIdResult
            {
                Result = new CustomerDTO
                {
                    Id           = objResult.Id,
                    FirstName    = objResult.FirstName,
                    IsMaker      = objResult.IsMaker,
                    LastName     = objResult.LastName,
                    IsVerified   = objResult.IsVerified,
                    ProfileImg   = objResult.ProfileImg,
                    About        = objResult.About,
                    IsOG         = objResult.IsOG,
                    IsOfficial   = objResult.IsOfficial,
                    DateJoined   = objResult.DateJoined,
                    Email        = objResult.Email,
                    PhoneNumber  = objResult.PhoneNumber,
                    ConnectionId = objResult.ConnectionId,
                    Handler      = objResult.Handler,
                    IsGuest      = objResult.IsGuest,

                },
                IsSuccess = true,
            });
        }
        catch (Exception ex)
        {
            return new JsonResult(new GetCustomerByIdResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("GetCustomerByHandler/{handler}")]
    [HttpGet]
    [ProducesResponseType(typeof(GetCustomerByIdResult), StatusCodes.Status201Created)]
    [AllowAnonymous]
    public async Task<IActionResult> GetMakerDetailByHandler(string handler)
    {
        try
        {
            var result = await getCustomerByHandler.ExecuteAsync(new Services.AccountService.Interactors.GetCustomerByHandlerArgs {
                Handler = handler
            });

            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new GetCustomerByIdResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }
            var objResult = result.Result;

            return new JsonResult(new GetCustomerByIdResult
            {
                Result = new CustomerDTO
                {
                    Id = objResult.Id,
                    FirstName = objResult.FirstName,
                    IsMaker = objResult.IsMaker,
                    IsVerified = objResult.IsVerified,
                    LastName = objResult.LastName,
                    ProfileImg = objResult.ProfileImg,
                    About = objResult.About,
                    IsOG = objResult.IsOG,
                    IsOfficial = objResult.IsOfficial,
                    DateJoined = objResult.DateJoined,
                    Email = objResult.Email
                },
                IsSuccess = true,
            });
        }
        catch (Exception ex)
        {
            return new JsonResult(new GetCustomerByIdResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("GetCustomerByEmail/{email}")]
    [HttpGet]
    [ProducesResponseType(typeof(GetCustomerByEmailResult), StatusCodes.Status200OK)]
    [AllowAnonymous]
    public async Task<IActionResult> GetCustomerByEmail(string email)
    {
        try
        {
            var result = await getCustomerByEmailHandler.ExecuteAsync(new Services.AccountService.Interactors.GetCustomerByEmailArgs
            {
                Email = email
            });

            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new GetCustomerByEmailResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }
            var objResult = result.Result;

            return new JsonResult(new GetCustomerByEmailResult
            {
                Result = new CustomerDTO
                {
                    IsVerified    = objResult.IsVerified,
                    Id            = objResult.Id,
                    Email         = objResult.Email,
                    FirstName     = objResult.FirstName,
                    About         = objResult.About,
                    Birthdate     = objResult.Birthdate,
                    DateJoined    = objResult.DateJoined,
                    ExternalLogin = objResult.ExternalLogin,
                    IsMaker       = objResult.IsMaker,
                    LastName      = objResult.LastName,
                    ProfileImg    = objResult.ProfileImg,
                    IsOfficial    = objResult.IsOfficial,
                    IsOG          = objResult.IsOG,
                    IsGuest       = objResult.IsGuest,
                },
                IsSuccess = true,
            });
        }
        catch (Exception ex)
        {
            return new JsonResult(new GetCustomerByEmailResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("GetWaitListByGuid/{guid}")]
    [HttpGet]
    [ProducesResponseType(typeof(GetWaitListByGuidResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetWaitListByGuid(string guid)
    {
        try
        {
            var result = await getWaitListByGuidHandler.ExecuteAsync(new Services.AccountService.Interactors.GetWaitListByGuidArgs
            {
                Guid = guid
            });

            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new GetWaitListByGuidResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }
            var objResult = result.Result;

            return new JsonResult(new GetWaitListByGuidResult
            {
                Result = new WaitlistDTO
                {
                    Email = result.Result.Email,
                    Guid = result.Result.Guid,
                    Id = result.Result.Id,
                    IsVerified = result.Result.IsVerified,
                    Token = result.Result.Token
                },
                IsSuccess = true,
            });
        }
        catch (Exception ex)
        {
            return new JsonResult(new GetWaitListByGuidResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("tMSSMcKhx9YpmcYCAfCkGnSfau8SE8")]
    [HttpPost]
    [ProducesResponseType(typeof(ExternalLoginResult), StatusCodes.Status200OK)]
    [AllowAnonymous]
    public async Task<IActionResult> HiddenExternalLogin([FromBody] ExternalLoginArgs args)
    {
        try
        {
            var result = await externalLoginHandler.ExecuteAsync(new Services.AccountService.Interactors.ExternalLoginArgs {
                Email = args.Email,
                FirstName = args.FirstName ?? string.Empty,
                LastName = args.LastName ?? string.Empty,
                IsEmptyUsername = args.IsEmptyUsername.HasValue ? args.IsEmptyUsername.Value : false
            });

            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new ExternalLoginResult { ErrorInfo = new ErrorInfo { Message = result.Message, Code = result.Error.Code } });
            }
            var objResult = result.Result;

            return new JsonResult(new ExternalLoginResult
            {
                Result = new VerifiedLoginDTO {
                    Email = objResult.Email,
                    ExternalLogin = objResult.ExternalLogin,
                    FirstName = objResult.FirstName,
                    GeneratedToken = objResult.GeneratedToken,
                    Id = objResult.Id,
                    IsMaker = objResult.IsMaker,
                    LastName = objResult.LastName,
                    IsNew = objResult.IsNew,
                    GeneratedNewToken = objResult.GeneratedNewToken,
                    GeneratedNewUid = objResult.GeneratedNewGuid,
                    IsEmptyUsername = objResult.IsEmptyUsername
                },
                IsSuccess = true,
            });
        }
        catch (Exception ex)
        {
            return new JsonResult(new ExternalLoginResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("GetExternalLoginDetail/{token}/{guid}")]
    [HttpGet]
    [ProducesResponseType(typeof(GetExternalLoginDetailResult), StatusCodes.Status200OK)]
    [AllowAnonymous]
    public async Task<IActionResult> GetExternalLoginDetail(string token, string guid)
    {
        try
        {
            var result = await getExternalLoginDetailHandler.ExecuteAsync(new Services.AccountService.Interactors.GetExternalLoginDetailArgs {
                Guid = guid,
                Token = token
            });

            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new GetExternalLoginDetailResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }
            var objResult = result.Result;

            return new JsonResult(new GetExternalLoginDetailResult
            {
                Result = new ExternalLoginDetailDTO {
                    Email = result.Result.Email,
                    FirstName = result.Result.FirstName,
                    LastName = result.Result.LastName,
                    IsEmptyUsername = result.Result.IsEmptyUsername
                },
                IsSuccess = true,
            });
        }
        catch (Exception ex)
        {
            return new JsonResult(new GetExternalLoginDetailResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("ResetPassword")]
    [HttpPost]
    [ProducesResponseType(typeof(ResetPasswordResult), StatusCodes.Status201Created)]
    [AllowAnonymous]

    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordArgs args)
    {
        try
        {
            var result = await resetPasswordHandler.ExecuteAsync(new Services.AccountService.Interactors.ResetPasswordArgs {
                Email = args.Email,
                ValidationRoute = args.ValidationRoute
            });
            if(!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new ResetPasswordResult {ErrorInfo = new ErrorInfo {Message = result.Message, Code = result.Error.Code}});
            }
            var created = result.Result;

            return new JsonResult(new ResetPasswordResult {
                Result = new Framework.ApiCommand.ApiCore.DTO.ResetPassword.ResetVerificationLinkDto {
                    Email = created.Email,
                    Id = created.Id,
                    VerificationLink = created.VerificationLink
                },
                IsSuccess = true
            });
        }
        catch (Exception ex)
        {
            return new JsonResult(new ResetPasswordResult {ErrorInfo = new ErrorInfo {Message = ex.Message}});
        }
    }
    [Route("ChangeEmailAddress")]
    [HttpPost]
    [ProducesResponseType(typeof(ChangEmailAddressResult), StatusCodes.Status201Created)]
    public async Task<IActionResult> ChangeEmailAddress([FromBody] ChangeEmailArgs args)
    {
        try
        {
            var result = await changeEmailHandler.ExecuteAsync(new Services.AccountService.Interactors.ChangeEmailArgs
            {
                CurrentEmail = args.CurrentEmail,
                NewEmail = args.NewEmail,
            });
            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new ChangEmailAddressResult { ErrorInfo = new ErrorInfo { Message = result.Message, Code = result.Error.Code } });
            }
            var created = result.Result;

            return new JsonResult(new ChangEmailAddressResult
            {
                Result = new Framework.ApiCommand.ApiCore.DTO.Customer.ChangeEmailDTO
                {
                    NewEmail = created.Email,
                },
                IsSuccess = true
            });
        }
        catch (Exception ex)
        {
            return new JsonResult(new ChangEmailAddressResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("VerifyResetPassword")]
    [HttpPost]
    [ProducesResponseType(typeof(VerifyResetPasswordResult), StatusCodes.Status201Created)]
    [AllowAnonymous]
    public async Task<IActionResult> VerifyResetPassword([FromBody] VerifyResetPasswordArgs args)
    {
        try
        {
            var result = await verifyResetPasswordHandler.ExecuteAsync(new Services.AccountService.Interactors.VerifyResetPasswordArgs {
                Guid = args.Guid,
                NewPassword = args.NewPassword,
                Token = args.Token
            });
            if(!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new VerifyResetPasswordResult {ErrorInfo = new ErrorInfo {Message = result.Message, Code = result.Error.Code}});
            }
            var created = result.Result;

            return new JsonResult(new VerifyResetPasswordResult {
                Result = new Framework.ApiCommand.ApiCore.DTO.ResetPassword.VerifyResetPaswordDto {
                    Success = true
                },
                IsSuccess = true
            });
        }
        catch (Exception ex)
        {
            return new JsonResult(new VerifyResetPasswordResult {ErrorInfo = new ErrorInfo {Message = ex.Message}});
        }
    }

    [Route("RequestRefund")]
    [HttpPost]
    [ProducesResponseType(typeof(RequestRefundResult), StatusCodes.Status201Created)]
    public async Task<IActionResult> RequestRefund([FromBody] RequestRefundArgs args)
    {
        try
        {
            var result = await requestRefundHandler.ExecuteAsync(new Services.AccountService.Interactors.RequestRefundArgs {
                PurchaseOrderId = args.PurchaseOrderId,
                Reason = args.Reason
            });
            if(!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new RequestRefundResult {ErrorInfo = new ErrorInfo {Message = result.Message, Code = result.Error.Code}});
            }
            var created = result.Result;

            return new JsonResult(new RequestRefundResult {
                Result = true,
                IsSuccess = true
            });
        }
        catch (Exception ex)
        {
            return new JsonResult(new RequestRefundResult {ErrorInfo = new ErrorInfo {Message = ex.Message}});
        }
    }

    [Route("GetRequestedRefunds")]
    [HttpGet]
    [ProducesResponseType(typeof(GetRequestedRefundsResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetRequestedRefunds([FromQuery] GetRequestedRefundsArgs args)
    {
        try
        {
            var result = await getRequestRefundHandler.ExecuteAsync(new Services.AccountService.Interactors.GetRequestRefundArgs {
                Status = args.Status,
                CountPerPage= args.CountPerPage,
                PageIndex= args.PageIndex,
                IsAdmin = args.IsAdmin,
                IncludeCustomer = args.IncludeCustomer,
                IncludePurchaseOrder= args.IncludePurchaseOrder
            });
            if(!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new GetRequestedRefundsResult {ErrorInfo = new ErrorInfo {Message = result.Message, Code = result.Error.Code}});
            }

            return new JsonResult(new GetRequestedRefundsResult {
                Result = result.Result.RequestedRefunds.Select(r => {
                    return new Framework.ApiCommand.ApiCore.DTO.PurchaseOrder.RequestedRefundDTO {
                        Id            = r.Id,
                        ActivityTitle = r.ExperienceTitle,
                        ReferenceNo   = r.ReferenceNumber,
                        Status        = r.Status,
                        Email         = r.Email,
                        FirstName     = r.FirstName,
                        LastName      = r.LastName,
                        Reason        = r.Reason,
                        OverAllTotal  = r.OverAllTotal,
                        RefundAmountGiven = r.RefundAmountGiven
                    };
                }),
                Pagination = result.Result.Pagination,
                IsSuccess = true
            });
        }
        catch (Exception ex)
        {
            return new JsonResult(new GetRequestedRefundsResult {ErrorInfo = new ErrorInfo {Message = ex.Message}});
        }
    }

    [Route("DeleteProfilePicture")]
    [HttpPost]
    [ProducesResponseType(typeof(DeleteProfilePictureResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteProfilePicture([FromBody] DeleteProfilePictureArgs args)
    {
        try
        {
            var result = await deleteProfilePictureHandler.ExecuteAsync(new Services.AccountService.Interactors.DeleteProfilePictureArgs
            {
            });
            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new DeleteProfilePictureResult { ErrorInfo = new ErrorInfo { Message = result.Message, Code = result.Error.Code } });
            }

            return new JsonResult(new DeleteProfilePictureResult
            {
                IsSuccess = true
            });
        }
        catch (Exception ex)
        {
            return new JsonResult(new DeleteProfilePictureResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("UpdatePayoutAccount")]
    [HttpPost]
    [ProducesResponseType(typeof(UpdatePayoutAccountResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdatePayoutAccount([FromBody] UpdatePayoutAccountArgs args)
    {
        try
        {
            var result = await createUpdatePayoutAccountHandler.ExecuteAsync(new Services.AccountService.Interactors.CreateUpdatePayoutAccountArgs {
                AccountHolder = args.AccountHolder,
                AccountNumber = args.AccountNumber,
                BankChannel = args.BankChannel
            });
            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new UpdatePayoutAccountResult { ErrorInfo = new ErrorInfo { Message = result.Message, Code = result.Error.Code } });
            }

            return new JsonResult(new UpdatePayoutAccountResult
            {
                Result = new PayoutAccountDTO {
                    AccountHolder = result.Result.AccountHolder,
                    AccountNumber = result.Result.AccountNumber,
                    Id = result.Result.Id,
                    BankChannel = result.Result.BankChannel
                },
                IsSuccess = true
            });
        }
        catch (Exception ex)
        {
            return new JsonResult(new UpdatePayoutAccountResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("GetPayoutAccount")]
    [HttpGet]
    [ProducesResponseType(typeof(GetPayoutAccountResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPayoutAccount()
    {
        try
        {
            var result = await getPayoutAccountHandler.ExecuteAsync(new Services.AccountService.Interactors.GetPayoutAccountArgs {});
            if(!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new GetPayoutAccountResult {ErrorInfo = new ErrorInfo {Message = result.Message, Code = result.Error.Code}});
            }
            
            var accountNumber = result.Result.AccountNumber;
            var maskedCount = accountNumber.Length < 5 ? accountNumber.Length : 5;

            var maskedAccount = new String('*', accountNumber.Length - maskedCount) + accountNumber.Substring(accountNumber.Length - maskedCount);

            return new JsonResult(new GetPayoutAccountResult {
                Result = new PayoutAccountDTO {
                    AccountHolder = result.Result.AccountHolder,
                    // masked the account number
                    AccountNumber = maskedAccount,
                    UnMaskAccountNumber = accountNumber,
                    Id = result.Result.Id,
                    BankChannel = result.Result.BankChannel
                },
                IsSuccess = true
            });
        }
        catch (Exception ex)
        {
            return new JsonResult(new GetPayoutAccountResult {ErrorInfo = new ErrorInfo {Message = ex.Message}});
        }
    }

    [Route("Customers")]
    [HttpGet]
    [ProducesResponseType(typeof(GetAllCustomerResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllCustomers([FromQuery] GetAllCustomersArgs args)
    {
        try
        {
            var result = await getAllCustomersHandler.ExecuteAsync(new Services.AccountService.Interactors.GetAllCustomersArgs
            {
                SearchValue = string.IsNullOrEmpty(args.SearchValue) ? string.Empty : args.SearchValue,
                CountPerPage = args.CountPerPage,
                PageIndex = args.PageIndex,
                IsOfficialPartner = args.IsOfficialPartner,
                HasVerification = args.HasVerification
            });

            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new GetAllCustomerResult { ErrorInfo = new ErrorInfo { Message = result.Message, Code = result.Error.Code } });
            }

            return new JsonResult(new GetAllCustomerResult
            {
                Result = result.Result.Customers.Select(c => {
                    return new CustomerDTO
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
                        IsVerifiedDate   = c.IsVerifiedDate,
                        IsOG             = c.IsOG,
                        IsOGDate         = c.IsOGDate,
                        IsOfficial       = c.IsOF,
                        IsOfficialDate   = c.IsOFDate,
                        CustomerPricing = new CustomerPricingDTO {
                            Rate = c.CustomerPricing != null ? c.CustomerPricing.Rate : 0,
                            IsManualPayment = c.CustomerPricing != null ? c.CustomerPricing.IsManualPayment : false,
                            InclusivePricing = c.CustomerPricing != null ? c.CustomerPricing.InclusivePricing : false
                        },
                        IsAccountBan     = c.IsAccountBan
                    };
                }),
                IsSuccess = true,
                Pagination = result.Result.Pagination
            });
        }
        catch (Exception ex)
        {
            return new JsonResult(new GetAllCustomerResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("Customer/Update")]
    [HttpPost]
    [ProducesResponseType(typeof(UpdateProfileDetailsResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateCustomerProfile([FromBody] UpdateProfileDetailsArgs args)
    {
        try
        {
            var result = await updateCustomerProfileHandler.ExecuteAsync(new Services.AccountService.Interactors.UpdateCustomerProfileArgs
            {
                VerifiedBadge = args.VerifiedBadge,
                IsVerifiedDate = args.VerifiedBadgeDate,
                CustomerId = args.CustomerId,
                IsOG = args.IsOG,
                IsOFDate = args.IsOFDate,
                IsOF = args.IsOF,
                IsOGDate = args.IsOGDate,
            });

            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new UpdateProfileDetailsResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new UpdateProfileDetailsResult
            {
                Result = new CustomerDTO
                {
                    FirstName = result.Result.FirstName,
                    LastName = result.Result.LastName,
                    IsVerified = result.Result.VerifiedBadge,
                    IsVerifiedDate = result.Result.IsVerifiedDate,
                    Id = result.Result.Id,
                    IsOG = result.Result.IsOG,
                    IsOGDate = result.Result.IsOGDate,
                    IsOfficial = result.Result.IsOF,
                    IsOfficialDate = result.Result.IsOFDate
                },
                IsSuccess = true
            });
        }
        catch (Exception ex)
        {
            return new JsonResult(new UpdateProfileDetailsResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("Refund/Update")]
    [HttpPost]
    [ProducesResponseType(typeof(UpdateRequestRefundResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateRefundRequest([FromBody] UpdateRequestRefundArgs args)
    {
        try
        {
            var result = await updateRequestRefundHandler.ExecuteAsync(new Services.AccountService.Interactors.UpdateRequestRefundArgs
            {
                RefundAmountGiven = args.RefundAmountGiven,
                RefundId          = args.RefundId,
                Status            = args.Status,
            });

            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new UpdateRequestRefundResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new UpdateRequestRefundResult
            {
               IsSuccess= true,
               Result = new RequestRefundDTO
               {
                   Status          = result.Result.Status,
                   CustomerId      = result.Result.CustomerId,
                   ExperienceTitle = result.Result.ExperienceTitle,
                   Id              = result.Result.Id,
                   PurchaseOrderId = result.Result.PurchaseOrderId,
                   Reason          = result.Result.Reason
               }
            });
        }
        catch (Exception ex)
        {
            return new JsonResult(new UpdateProfileDetailsResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("SubmitAccountVerified")]
    [HttpPost]
    [ProducesResponseType(typeof(SubmitAccountVerifiedResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> SubmitAccountVerified()
    {
        try
        {
            var result = await accountSubmitVerifiedHandler.ExecuteAsync(new Services.AccountService.Interactors.AccountSubmitVerifiedArgs {});

            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new SubmitAccountVerifiedResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new SubmitAccountVerifiedResult
            {
               IsSuccess= true,
               Result = true
            });
        }
        catch (Exception ex)
        {
            return new JsonResult(new SubmitAccountVerifiedResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("Customer/ConnectionId/Update")]
    [HttpPost]
    [ProducesResponseType(typeof(UpdateConnectionIdResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateConnectionId([FromBody] UpdateConnectionIdArgs args)
    {
        try
        {
            var result = await updateConnectionIdHandler.ExecuteAsync(new Services.AccountService.Interactors.UpdateConnectionIdArgs
            {
                ConnectionId = args.ConnectionId
            });

            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new UpdateConnectionIdResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new UpdateConnectionIdResult
            {
                Result = new CustomerDTO
                {
                    Id = result.Result.CustomerId,
                    ConnectionId = result.Result.ConnectionId
                },
                IsSuccess = true
            });
        }
        catch (Exception ex)
        {
            return new JsonResult(new UpdateConnectionIdResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("Customer/Verification/Send")]
    [HttpPost]
    [ProducesResponseType(typeof(VerifyUserNotificationResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> NotifyCustomerVerification([FromBody] VerifyUserNotificationArgs args)
    {
        try
        {
            var result = await verifyUserNotificationHandler.ExecuteAsync(new Services.AccountService.Interactors.VerifyUserNotificationArgs
            {
                Email = args.Email,
                BankDetails = args.BankDetails,
                IdAttached = args.IdAttached
            });

            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new VerifyUserNotificationResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new VerifyUserNotificationResult
            {
                Result = result.Succeeded,
                IsSuccess = true
            });
        }
        catch (Exception ex)
        {
            return new JsonResult(new VerifyUserNotificationResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("BlockAccount")]
    [HttpPost]
    [ProducesResponseType(typeof(BlockedAccountResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> BlockAccount([FromBody] BlockedAccountArgs args)
    {
        try
        {
            var result = await blockedAccountHandler.ExecuteAsync(new Services.AccountService.Interactors.BlockedAccountArgs {
                Id = args.CustomerId,
                IsBlock = args.IsBlock
            });

            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new BlockedAccountResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new BlockedAccountResult
            {
               IsSuccess= true,
               Result = true
            });
        }
        catch (Exception ex)
        {
            return new JsonResult(new BlockedAccountResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [AllowAnonymous]
    [Route("1UnCvQdTzi8dUHqKWgZGE1Xf7zqDo7EW99shdKGd2xddj4mZLg9UHJhuuYM3")]
    [HttpPost]
    [ProducesResponseType(typeof(SecretLoginResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> SecretLogin([FromBody] SecretLoginArgs args)
    {
        try
        {
            var result = await extraLoginHandler.ExecuteAsync(new Services.AccountService.Interactors.ExtraLoginArgs {
                Email = args.Email,
                Password = args.Password
            });
            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new SecretLoginResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }
            var loginResult = result.Result;

            return new JsonResult(new SecretLoginResult
            {
               IsSuccess= true,
               Result = new VerifiedLoginDTO {
                Email = loginResult.Email,
                ExternalLogin = loginResult.ExternalLogin,
                FirstName = loginResult.FirstName,
                Id = loginResult.Id,
                GeneratedToken = loginResult.GeneratedToken,
                IsMaker = loginResult.IsMaker,
                LastName = loginResult.LastName
               }
            });
        }
        catch (Exception ex)
        {
            return new JsonResult(new SecretLoginResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("DeleteWaitlist")]
    [HttpPost]
    [ProducesResponseType(typeof(DeleteWaitlistResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteAddOnById([FromBody] DeleteWaitlistArgs args)
    {
        try
        {
            var deleteResult = await deleteWaitlistHandler.ExecuteAsync(new Services.AccountService.Interactors.DeleteWaitlistArgs
            {
                Email = args.Email
            });

            if (!deleteResult.Succeeded || deleteResult.Result == null)
            {
                return new JsonResult(new DeleteWaitlistResult { ErrorInfo = new ErrorInfo { Message = deleteResult.Message } });
            }

            var result = deleteResult.Result;

            return new JsonResult(new DeleteWaitlistResult
            {
                IsSuccess = result.IsSuccess
            });
        }
        catch (Exception ex)
        {
            return new JsonResult(new DeleteWaitlistResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }


    [Route("SendOTP")]
    [HttpPost]
    [ProducesResponseType(typeof(SendOTPResult), StatusCodes.Status201Created)]
    [AllowAnonymous]
    public async Task<IActionResult> SendOTP([FromBody] SendOTPArgs args)
    {
        try
        {
            var result = await sendOTPHandler.ExecuteAsync(new Services.AccountService.Interactors.SendOTPArgs
            {
                Email = args.Email,
                OTPCode = args.OTPCode
            });

            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new SendOTPResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }
            var objResult = result.Result;

            return new JsonResult(new SendOTPResult
            {
                Result = new OtpDTO
                {
                    Email = objResult.Email,
                    OTPcode = objResult.OTPCode
                },
                IsSuccess = true,
            });
        }
        catch (Exception ex)
        {
            return new JsonResult(new SendOTPResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("GetOTPs")]
    [HttpGet]
    [ProducesResponseType(typeof(GetUserOTPResult), StatusCodes.Status200OK)]
    [AllowAnonymous]
    public async Task<IActionResult> GetOTPByEmail([FromQuery] GetUserOTPArgs args)
    {
        try
        {
            var result = await getUserOTPHandler.ExecuteAsync(new Services.AccountService.Interactors.GetUserOTPArgs
            {
                Email = args.Email
            });
            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new GetUserOTPResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }
            return new JsonResult(new GetUserOTPResult
            {
                IsSuccess = true,
                Result = result.Result.GuestOTPs.Select(e =>
                {
                    return new Framework.ApiCommand.ApiCore.DTO.OTP.OtpDTO
                    {
                        Email = e.Email,
                        OtpCode = e.OTPCode,
                        CreatedOn = e.CreatedOn
                    };
                })
            });
        }
        catch (Exception ex)
        {
            return new JsonResult(new GetActivityImagesResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("VerifyEmail")]
    [HttpPost]
    [ProducesResponseType(typeof(VerifyEmailResult), StatusCodes.Status201Created)]
    [AllowAnonymous]
    public async Task<IActionResult> VerifyEmail([FromBody] VerifyEmailArgs args)
    {
        try
        {
            var result = await verifyOTPHandler.ExecuteAsync(new Services.AccountService.Interactors.VerifyOTPArgs
            {
                Email = args.Email,
            });

            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new VerifyEmailResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }
            var objResult = result.Result;

            return new JsonResult(new VerifyEmailResult
            {
                Result = new OtpDTO
                {
                    Email = objResult.Email
                },
                IsSuccess = true,
            });
        }
        catch (Exception ex)
        {
            return new JsonResult(new VerifyEmailResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("CreateGuestCustomer")]
    [HttpPost]
    [ProducesResponseType(typeof(CreateGuestCustomerResult), StatusCodes.Status201Created)]
    [AllowAnonymous]
    public async Task<IActionResult> CreateGuestCustomer([FromBody] CreateGuestCustomerArgs args)
    {
        try
        {
            var result = await createGuestCustomerHandler.ExecuteAsync(
                new Services.AccountService.Interactors.CreateGuestCustomerArgs 
                {
                    FirstName = args.FirstName,
                    LastName = args.LastName,
                    Email = args.Email,
                    Birthdate = args.Birthdate,
                    PhoneNumber = args.PhoneNumber,
                    About = args.About,
                    ProfilePath = args.ProfilePath,
                    Handler = args.Handler,
                    HasAcceptedTerms = args.HasAcceptedTerms
                });

            if (!result.Succeeded || result.Result is null)
            {
                return new JsonResult(new CreateGuestCustomerResult 
                { 
                    ErrorInfo = new ErrorInfo { Message = result.Message }
                });
            }

            return new JsonResult(new CreateGuestCustomerResult 
            {
                Result = result.Result.SessionToken,
                IsSuccess = true
            });
        }
        catch (Exception ex)
        {
            return new JsonResult(new CreateGuestCustomerResult 
            { 
                ErrorInfo = new ErrorInfo { Message = ex.Message }
            });
        }
    }
}