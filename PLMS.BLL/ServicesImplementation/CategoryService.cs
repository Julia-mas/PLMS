using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PLMS.BLL.DTO.CategoriesDto;
using PLMS.BLL.Filters;
using PLMS.BLL.ServicesInterfaces;
using PLMS.Common.Exceptions;
using PLMS.DAL.Entities;
using PLMS.DAL.Interfaces;
using System.Linq.Expressions;
using Task = System.Threading.Tasks.Task;

namespace PLMS.BLL.ServicesImplementation
{
    public class CategoryService : ICategoryService
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IRepository<Category> _categoryRepository;
        private readonly IPermissionService _permissionService;

        public CategoryService(IUnitOfWork unitOfWork, IMapper mapper, IPermissionService permissionService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _categoryRepository = _unitOfWork.GetRepository<Category>();
            _permissionService = permissionService;
        }

        public async Task<int> AddCategoryAsync(AddCategoryDto categoryDto)
        {
            var category = await _categoryRepository.GetByPredicateAsync(c => c.Title == categoryDto.Title && c.UserId == categoryDto.UserId );
            if (category != null)
            {
                return category.Id;
            }

            category = _mapper.Map<Category>(categoryDto);

            await _categoryRepository.CreateAsync(category);
            await _unitOfWork.CommitChangesToDatabaseAsync();

            return category.Id;
        }

        public async Task EditCategoryAsync(EditCategoryDto categoryDto, string userId)
        {
            var category = await _categoryRepository.GetByPredicateAsync(c => c.Id == categoryDto.Id);

            if (category == null || !_permissionService.HasPermissionToCategory(category, userId))
            {
                throw new NotFoundException("Category was not found.");
            }

            category.Title = categoryDto.Title != default ? categoryDto.Title : category.Title;

            _categoryRepository.Update(category);
            await _unitOfWork.CommitChangesToDatabaseAsync();
        }

        public async Task DeleteCategoryAsync(int id, string userId)
        {
            Category? category = await _categoryRepository.GetByPredicateAsync(c => c.Id == id, c => c.Goals);

            if (category == null || !_permissionService.HasPermissionToCategory(category, userId))
            {
                return;
            }
            _categoryRepository.Remove(category);
            await _unitOfWork.CommitChangesToDatabaseAsync();
        }

        public async Task<GetCategoryDto> GetCategoryByIdAsync(int id, string userId)
        {
            var category = await _categoryRepository.GetByPredicateAsync(c => c.Id == id, c => c.Goals);

            if (category == null || category.UserId != userId)
            {
                throw new NotFoundException("Category was not found");
            }

            var categoryDto = _mapper.Map<GetCategoryDto>(category);

            return categoryDto;
        }

        public async Task<List<GetCategoryDto>> GetCategorieFilteredAsync(CategoryFilter filters)
        {
            var filterExpression = ApplyFilters(filters);
            var orderFunction = ApplyOrder(filters);

            // Apply filtation and sorting on the DB level.
            var query = _categoryRepository.GetAll().Include(c => c.Goals).Where(filterExpression);

            var orderedQuery = orderFunction(query);

            // Pagination
            var paginatedQuery = orderedQuery.Skip((filters.PageNumber - 1) * filters.ItemsPerPageCount)
                                             .Take(filters.ItemsPerPageCount);

            // Call to the DB
            var categories = await paginatedQuery.ToListAsync();

            var categoriesDto = _mapper.Map<List<GetCategoryDto>>(categories);

            return categoriesDto;
        }

        private static Func<IQueryable<Category>, IOrderedQueryable<Category>> ApplyOrder(CategoryFilter filters)
        {
            return q => filters.SortOrder == BaseFilter.SortOrders.Asc ? q.OrderBy(g => g.Title) : q.OrderByDescending(g => g.Title);
        }

        private static Expression<Func<Category, bool>> ApplyFilters(CategoryFilter filters)
        {
            return g => g.UserId.Contains(filters.UserId);
        }
    }
}
