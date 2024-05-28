using AutoMapper;
using LibraryManagementSystem.Bll.Interfaces.Library;
using LibraryManagementSystem.Common.Runtime;
using LibraryManagementSystem.Dal.Repository.Interfaces.Library;
using LibraryManagementSystem.Entities.Dtos.Library;
using LibraryManagementSystem.Entities.Models.Library;

namespace LibraryManagementSystem.Bll.Implements.Library
{
    public class SubCategoryBll(ISubCategoryRepository repository, ICategoryBll categoryBll, IMapper mapper) : ISubCategoryBll
    {
        private readonly ISubCategoryRepository _repository = repository;
        private readonly ICategoryBll _categoryBll = categoryBll;
        private readonly IMapper _mapper = mapper;

        public async Task<ApiResponse> Create(SubCategory entity) => await _repository.Create(entity);

        public async Task<ApiResponse> CreateMany(IEnumerable<SubCategory> entities)
        {
            var response = await _repository.CreateMany(entities);
            // Comprobar si hay elementos
            if (response.Result is null || response.Result is not IEnumerable<SubCategory> subCategories)
                return response;

            // Obtener todos los ids de las categorias
            var categoryIds = subCategories.Select(sc => sc.CategoryId).ToList();
            // Convertir subCategories a SubCategoryDto
            var subCategoriesDto = _mapper.Map<IEnumerable<SubCategoryDto>>(subCategories).ToList();

            // 1. Comprobar si hay categorias
            var categoriesResponse = _categoryBll.GetAll();
            if (categoriesResponse.Result.Result is not null && categoriesResponse.Result.Result is IEnumerable<CategoryDto> categories)
            {
                var categoriesDictionary = categories.ToDictionary(c => c.CategoryId);

                for (int i = 0; i < subCategoriesDto.Count; i++)
                {
                    var subCategoryDto = subCategoriesDto[i];
                    var categoryId = categoryIds[i];

                    if (categoriesDictionary.TryGetValue(categoryId, out var categoryDto)) { subCategoryDto.Category = categoryDto; }
                }
            }

            // Retornar Dtos
            response.Result = subCategoriesDto;

            return response;
        }

        public async Task<ApiResponse> Delete(int id) => await _repository.Delete(id);

        public async Task<ApiResponse> GetAll()
        {
            var response = await _repository.GetAll();
            // Comprobar si hay elementos
            if (response.Result is null || response.Result is not IEnumerable<SubCategory> subCategories)
                return response;

            // Obtener todos los ids de las categorias
            var categoryIds = subCategories.Select(sc => sc.CategoryId).ToList();
            // Convertir subCategories a SubCategoryDto
            var subCategoriesDto = _mapper.Map<IEnumerable<SubCategoryDto>>(subCategories).ToList();

            // 1. Comprobar si hay categorias
            var categoriesResponse = _categoryBll.GetAll();
            if (categoriesResponse.Result.Result is not null && categoriesResponse.Result.Result is IEnumerable<CategoryDto> categories)
            {
                var categoriesDictionary = categories.ToDictionary(c => c.CategoryId);

                for (int i = 0; i < subCategoriesDto.Count; i++)
                {
                    var subCategoryDto = subCategoriesDto[i];
                    var categoryId = categoryIds[i];

                    if (categoriesDictionary.TryGetValue(categoryId, out var categoryDto)) { subCategoryDto.Category = categoryDto; }
                }
            }

            // Retornar Dtos
            response.Result = subCategoriesDto;

            return response;
        }

        public async Task<ApiResponse> GetById(int id)
        {
            var response = await _repository.GetById(id);
            // Comprobar si hay un elemento
            if (response.Result is null || response.Result is not SubCategory subCategory)
                return response;

            // Obtener id de la categoria
            var categoryId = subCategory.CategoryId;
            // Convertir subCategory a SubCategoryDto
            var subCategoryDto = _mapper.Map<SubCategoryDto>(subCategory);

            // 1. Comprobar si existe la categoria
            var categoryResponse = _categoryBll.GetById(categoryId);
            if (categoryResponse.Result.Result is not null && categoryResponse.Result.Result is CategoryDto category)
            {
                subCategoryDto.Category = category;
            }

            // Retornar Dto
            response.Result = subCategoryDto;

            return response;
        }

        public async Task<ApiResponse> Update(SubCategory entity) => await _repository.Update(entity);

        public async Task<ApiResponse> UpdateMany(IEnumerable<SubCategory> entities) => await _repository.UpdateMany(entities);
    }
}
