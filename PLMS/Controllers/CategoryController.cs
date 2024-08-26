using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PLMS.API.ApiHelper;
using PLMS.API.Models.ModelsCategories;
using PLMS.BLL.DTO.CategoriesDto;
using PLMS.BLL.Filters;
using PLMS.BLL.ServicesInterfaces;
using PLMS.Common.Exceptions;
using System.Security.Claims;

namespace PLMS.API.Controllers
{
    [Authorize]
    [Route("plms/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;

        public CategoryController(ICategoryService categoryService, IMapper mapper)
        {
            _categoryService = categoryService;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult<CategoryBaseModel>> Add(CategoryBaseModel model)
        {
            if (!ModelState.IsValid)
            {
                var errorMessage = string.Join(", ", ModelState.Values.First().Errors.First().ErrorMessage);
                return ApiResponseHelper.CreateErrorResponse(errorMessage, StatusCodes.Status400BadRequest);
            }
            var categoryDto = _mapper.Map<AddCategoryDto>(model);
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            categoryDto.UserId = userId;
            int categoryId;

            try
            {
                categoryId = await _categoryService.AddCategoryAsync(categoryDto);
            }
            catch (NotFoundException ex)
            {
                return ApiResponseHelper.CreateErrorResponse(ex.Message, StatusCodes.Status404NotFound);
            }

            return ApiResponseHelper.CreateOkResponseWithMessage("Category was added successfully", categoryId);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Edit(CategoryBaseModel model, int id)
        {
            if (!ModelState.IsValid)
            {
                return ApiResponseHelper.CreateValidationErrorResponse(ModelState);
            }
            var categoryDto = _mapper.Map<EditCategoryDto>(model);
            categoryDto.Id = id;

            try
            {
                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                await _categoryService.EditCategoryAsync(categoryDto, userId);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return ApiResponseHelper.CreateErrorResponse(ex.Message, StatusCodes.Status500InternalServerError);
            }
            catch (NotFoundException ex)
            {
                return ApiResponseHelper.CreateErrorResponse(ex.Message, StatusCodes.Status404NotFound);
            }

            return ApiResponseHelper.CreateOkResponseWithMessage<string>("Category was updated successfully");
        }

        [HttpDelete]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                await _categoryService.DeleteCategoryAsync(id, userId);
            }
            catch (NotFoundException ex)
            {
                return ApiResponseHelper.CreateErrorResponse(ex.Message, StatusCodes.Status404NotFound);
            }

            return ApiResponseHelper.CreateOkResponseWithMessage<string>("Category was deleted successfully");
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GetCategoryViewModel>> GetById(int id)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            GetCategoryDto categoryDto;
            try
            {
                categoryDto = await _categoryService.GetCategoryByIdAsync(id, userId);
            }
            catch (NotFoundException ex)
            {
                return ApiResponseHelper.CreateErrorResponse(ex.Message, StatusCodes.Status404NotFound);
            }

            if (!ModelState.IsValid)
            {
                ApiResponseHelper.CreateValidationErrorResponse(ModelState);
            }

            var categoryModel = _mapper.Map<GetCategoryViewModel>(categoryDto);

            return ApiResponseHelper.CreateOkResponseWithoutMessage(categoryModel);
        }

        [HttpGet("GetFilteredCategories")]
        public async Task<ActionResult<IEnumerable<GetCategoryViewModel>>> GetFilteredFull([FromQuery] CategoryFilter filters)
        {
            filters.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(filters.UserId))
            {
                return ApiResponseHelper.CreateErrorResponse("Category were not found!", StatusCodes.Status404NotFound);
            }

            var categoryDto = await _categoryService.GetCategorieFilteredAsync(filters);
            var categoryModel = categoryDto.Select(g => _mapper.Map<GetCategoryViewModel>(g));

            return ApiResponseHelper.CreateOkResponseWithoutMessage(categoryModel);
        }
    }
}
