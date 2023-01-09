using Microsoft.AspNetCore.Mvc;
using Cinnamon.Api.Core.Services.AccountService.Handlers;
using Cinnamon.Framework.ApiCommand.ApiCore.Account.Request;
using Cinnamon.Framework.ApiCommand.ApiCore.Account.Response;
using Cinnamon.Framework.ApiCommand.ApiCore;
using Cinnamon.Framework.ApiCommand.ApiCore.DTO.Customer;
using Cinnamon.Framework.ApiCommand.ApiCore.DTO.Waitlist;
using Microsoft.AspNetCore.Authorization;
using Cinnamon.Framework.ApiCommand.ApiCore.DTO.FamilyMember;

namespace Cinnamon.Api.Core.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class AccountController : ControllerBase 
{
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

    public AccountController(ISubmitRegisterHandler submitRegisterHandler, ISubmitWaitlistHandler submitWaitlistHandler,
        ISubmitVerifyEmailHandler submitVerifyEmailHandler, ISubmitLoginHandler submitLoginHandler,
        ISubmitResendVerificationHandler submitResendEmailHandler, IGetProfileHandler getProfileHandler,
        IGetFamilyMembersHandler getFamilyMembersHandler, IUpdateFamilyMembersHandler updateFamilyMembersHandler,
        ICreateFamilyMembersHandler createFamilyMembersHandler, IDeleteFamilyMembersHandler deleteFamilyMembersHandler,
        ISubmitUpdateProfileHandler updateProfileHandler, IGetGovernmentIdsHandler getGovernmentIdsHandler,
        IUploadGovernmentIdHandler uploadGovernmentIdHandler 
        // IUploadProfilePictureHandler uploadProfilePictureHandler,
        // IGetProfilePictureHandler getProfilePictureHandler, 
        //IGetWaitListHandler getWaitListHandler
        )
    {
        this.submitRegisterHandler = submitRegisterHandler;
        this.submitWaitlistHandler = submitWaitlistHandler;
        this.submitVerifyEmailHandler = submitVerifyEmailHandler;
        this.submitLoginHandler = submitLoginHandler;
        this.submitResendEmailHandler = submitResendEmailHandler;
        this.getProfileHandler = getProfileHandler;
        this.getFamilyMembersHandler = getFamilyMembersHandler;
        this.updateFamilyMembersHandler = updateFamilyMembersHandler;
        this.createFamilyMembersHandler = createFamilyMembersHandler;
        this.deleteFamilyMembersHandler = deleteFamilyMembersHandler;
        this.updateProfileHandler = updateProfileHandler;
        this.getGovernmentIdsHandler = getGovernmentIdsHandler;
        this.uploadGovernmentIdHandler = uploadGovernmentIdHandler;
        this.uploadProfilePictureHandler = uploadProfilePictureHandler;
        this.getProfilePictureHandler = getProfilePictureHandler;   
        this.getWaitListHandler = getWaitListHandler;
    }

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
                FirstName = args.FirstName,
                LastName = args.LastName,
                Password = args.Password,
                ProfilePath = args.ProfilePath
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
                    ExternalLogin = objResult.ExternalLogin,
                    FirstName = objResult.FirstName,
                    LastName = objResult.LastName,
                    ProfileImg = objResult.ProfileImg,
                    Id = objResult.Id
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
                ValidationRoute = args.ValidationRoute
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
                    Email = profile.Email,
                    FirstName = profile.FirstName,
                    LastName = profile.LastName,
                    IsMaker = profile.IsMaker,
                    Id = profile.Id,
                    About = profile.About,
                    Birthdate = profile.Birthdate,
                    IsVerified = profile.IsVerified,
                    ProfileImg = profile.ProfileImagePath
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
                About = args.About,
                Birthdate = args.Datebirth,
                FirstName = args.FirstName,
                LastName = args.LastName,
            });

            if(!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new UpdateProfileDetailsResult {ErrorInfo = new ErrorInfo {Message = result.Message}});
            }

            return new JsonResult(new UpdateProfileDetailsResult {
                Result = new CustomerDTO {
                    About = result.Result.About,
                    Birthdate = result.Result.Birthdate,
                    FirstName = result.Result.FirstName,
                    LastName = result.Result.LastName,
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
}