using Cinnamon.Api.Data.Models.Subcategory.Response;
using Cinnamon.Api.Data.Services.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;

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
                    return new JsonResult(new GetSubcategorytResult { ErrorInfo = new Models.ErrorInfo { Message = result.Message } });
                }

                return new JsonResult(new GetSubcategorytResult { Result = result.Result, IsSuccess = true });
            }
            catch (Exception ex)
            {
                return new JsonResult(new GetSubcategorytResult { ErrorInfo = new Models.ErrorInfo { Message = ex.Message } });
            }
        }
    }
}
