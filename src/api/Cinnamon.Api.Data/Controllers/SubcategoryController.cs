using Cinnamon.Api.Data.Services.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Cinnamon.Framework.ApiCommand.ApiData;
using Cinnamon.Framework.ApiCommand.ApiData.Subcategory.Response;
using Cinnamon.Framework.ApiCommand.ApiData.Subcategory.Request;

namespace Cinnamon.Api.Data.Controllers
{
    public class SubcategoryController : ControllerBase
    {
        private readonly ISubCategoryRepository subCategoryRepository;
        public SubcategoryController(ISubCategoryRepository subCategoryRepository)
        {
            this.subCategoryRepository = subCategoryRepository;
        }
        [Route("GetSubCategoryById/{id}")]
        [HttpGet]
        [ProducesResponseType(typeof(GetSubcategorytResult), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSubCategorybyId(int id)
        {
            try
            {
                var result = await subCategoryRepository.GetByIdAsync(id);
                if (!result.Succeeded || result.Result == null)
                {
                    return new JsonResult(new GetSubcategorytResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
                }

                return new JsonResult(new GetSubcategorytResult { Result = result.Result, IsSuccess = true });
            }
            catch (Exception ex)
            {
                return new JsonResult(new GetSubcategorytResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
            }
        }
        [Route("GetAllSubCategory")]
        [HttpGet]
        [ProducesResponseType(typeof(GetAllSubcategoryResult), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllSubCategory([FromQuery] GetAllSubcategoryArgs args)
        {
            try
            {
                var result =
                args.PageIndex.HasValue && args.CountPerPage.HasValue ?
                   await subCategoryRepository.GetAllAsync(args.CountPerPage, (args.PageIndex - 1) * args.CountPerPage) :
                   await subCategoryRepository.GetAllAsync();
                if (!result.Succeeded || result.Result == null)
                {
                    return new JsonResult(new GetAllSubcategoryResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
                }
                // get all without pagination to get all rows
                var all = args.PageIndex.HasValue && args.CountPerPage.HasValue ?
                await subCategoryRepository.GetAllAsync(null, null) :
                await subCategoryRepository.GetAllAsync();

                if (!all.Succeeded || all.Result == null)
                {
                    return new JsonResult(new GetAllSubcategoryResult { ErrorInfo = new ErrorInfo { Message = all.Message } });
                }

                var totalRecords = all.Result.Count();
                return new JsonResult(new GetAllSubcategoryResult
                {
                    Result = result.Result,
                    IsSuccess = true,
                    Pagination = new Pagination
                    {
                        PageIndex = args.PageIndex,
                        PerPage = args.CountPerPage,
                        TotalRecords = totalRecords,
                        TotalPages = args.CountPerPage.HasValue && args.PageIndex.HasValue ?
                                    (int)Math.Ceiling(Convert.ToDouble(totalRecords / args.CountPerPage.Value)) : null
                    }
                });
            }
            catch (Exception ex)
            {
                return new JsonResult(new GetAllSubcategoryResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
            }
        }
        [Route("CreateSubCategory")]
        [HttpPost]
        [ProducesResponseType(typeof(CreatedSubcategoryResult), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateSubCategory([FromBody] CreateSubcategoryArgs args)
        {
            try
            {
                var result = await subCategoryRepository.CreateSubCategoryAsync(args.CategoryId, args.SubcategoryName);
                if (!result.Succeeded || result.Result == null)
                {
                    return new JsonResult(new CreatedSubcategoryResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
                }

                return new JsonResult(new CreatedSubcategoryResult { IsSuccess = true, Result = result.Result });
            }
            catch (Exception ex)
            {
                return new JsonResult(new CreatedSubcategoryResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
            }
        }
        [Route("UpdateSubCategory")]
        [HttpPost]
        [ProducesResponseType(typeof(UpdateSubcategoryResult), StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateSubCategory([FromBody] UpdateSubcategoryArgs args)
        {
            try
            {
                var result = await subCategoryRepository.UpdateSubCategoryAsync(args.SubcategoryId, args.CategoryId, args.SubcategoryName);

                if (!result.Succeeded || result.Result == null)
                {
                    return new JsonResult(new UpdateSubcategoryResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
                }

                return new JsonResult(new UpdateSubcategoryResult { IsSuccess = true, Result = result.Result });
            }
            catch (Exception ex)
            {
                return new JsonResult(new UpdateSubcategoryResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
            }
        }
    }
}
