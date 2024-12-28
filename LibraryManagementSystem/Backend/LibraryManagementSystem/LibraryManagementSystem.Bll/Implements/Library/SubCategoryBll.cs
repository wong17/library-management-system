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
        public async Task<ApiResponse> Create(SubCategoryInsertDto entity) => await repository.Create(mapper.Map<SubCategory>(entity));

        public async Task<ApiResponse> CreateMany(IEnumerable<SubCategoryInsertDto> entities)
        {
            var response = await repository.CreateMany(mapper.Map<IEnumerable<SubCategory>>(entities));
            // Comprobar si hay elementos
            if (response.Result is not IEnumerable<SubCategory> subCategories)
                return response;

            // Obtener todos los ids de las categorias
            var categoryIds = subCategories.Select(sc => sc.CategoryId).ToList();
            // Convertir subCategories a SubCategoryDto
            var subCategoriesDto = mapper.Map<IEnumerable<SubCategoryDto>>(subCategories).ToList();

            // 1. Comprobar si hay categorias
            var categoriesResponse = categoryBll.GetAll();
            if (categoriesResponse.Result.Result is IEnumerable<CategoryDto> categories)
            {
                var categoriesDictionary = categories.ToDictionary(c => c.CategoryId);

                for (var i = 0; i < subCategoriesDto.Count; i++)
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

        public async Task<ApiResponse> Delete(int id) => await repository.Delete(id);

        public async Task<ApiResponse> GetAll()
        {
            var response = await repository.GetAll();
            // Comprobar si hay elementos
            if (response.Result is not IEnumerable<SubCategory> subCategories)
                return response;

            // Obtener todos los ids de las categorias
            var categoryIds = subCategories.Select(sc => sc.CategoryId).ToList();
            // Convertir subCategories a SubCategoryDto
            var subCategoriesDto = mapper.Map<IEnumerable<SubCategoryDto>>(subCategories).ToList();

            // 1. Comprobar si hay categorias
            var categoriesResponse = categoryBll.GetAll();
            if (categoriesResponse.Result.Result is IEnumerable<CategoryDto> categories)
            {
                var categoriesDictionary = categories.ToDictionary(c => c.CategoryId);

                for (var i = 0; i < subCategoriesDto.Count; i++)
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
            var response = await repository.GetById(id);
            // Comprobar si hay un elemento
            if (response.Result is not SubCategory subCategory)
                return response;

            // Obtener id de la categoria
            var categoryId = subCategory.CategoryId;
            // Convertir subCategory a SubCategoryDto
            var subCategoryDto = mapper.Map<SubCategoryDto>(subCategory);

            // 1. Comprobar si existe la categoria
            var categoryResponse = categoryBll.GetById(categoryId);
            if (categoryResponse.Result.Result is CategoryDto category)
            {
                subCategoryDto.Category = category;
            }

            // Retornar Dto
            response.Result = subCategoryDto;

            return response;
        }

        public async Task<ApiResponse> Update(SubCategoryUpdateDto entity) => await repository.Update(mapper.Map<SubCategory>(entity));

        public async Task<ApiResponse> UpdateMany(IEnumerable<SubCategoryUpdateDto> entities) =>
            await repository.UpdateMany(mapper.Map<IEnumerable<SubCategory>>(entities));
    }
}