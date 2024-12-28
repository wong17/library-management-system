using AutoMapper;
using LibraryManagementSystem.Bll.Interfaces.Library;
using LibraryManagementSystem.Common.Runtime;
using LibraryManagementSystem.Dal.Repository.Interfaces.Library;
using LibraryManagementSystem.Entities.Dtos.Library;
using LibraryManagementSystem.Entities.Models.Library;

namespace LibraryManagementSystem.Bll.Implements.Library
{
    public class CategoryBll(ICategoryRepository repository, IMapper mapper) : ICategoryBll
    {
        public async Task<ApiResponse> Create(CategoryInsertDto entity) => await repository.Create(mapper.Map<Category>(entity));

        public async Task<ApiResponse> CreateMany(IEnumerable<CategoryInsertDto> entities)
        {
            var response = await repository.CreateMany(mapper.Map<IEnumerable<Category>>(entities));
            // Comprobar si hay elementos
            if (response.Result is not IEnumerable<Category> category)
                return response;
            // Retornar Dtos
            response.Result = mapper.Map<IEnumerable<CategoryDto>>(category);

            return response;
        }

        public async Task<ApiResponse> Delete(int id) => await repository.Delete(id);

        public async Task<ApiResponse> GetAll()
        {
            var response = await repository.GetAll();
            // Comprobar si hay elementos
            if (response.Result is not IEnumerable<Category> category)
                return response;
            // Retornar Dtos
            response.Result = mapper.Map<IEnumerable<CategoryDto>>(category);

            return response;
        }

        public async Task<ApiResponse> GetById(int id)
        {
            var response = await repository.GetById(id);
            // Comprobar si hay un elemento
            if (response.Result is not Category category)
                return response;
            // Retornar Dto
            response.Result = mapper.Map<CategoryDto>(category);

            return response;
        }

        public async Task<ApiResponse> Update(CategoryUpdateDto entity) => await repository.Update(mapper.Map<Category>(entity));

        public async Task<ApiResponse> UpdateMany(IEnumerable<CategoryUpdateDto> entities) =>
            await repository.UpdateMany(mapper.Map<IEnumerable<Category>>(entities));
    }
}