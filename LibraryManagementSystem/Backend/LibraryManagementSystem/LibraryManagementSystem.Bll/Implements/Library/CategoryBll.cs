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
        private readonly ICategoryRepository _repository = repository;
        private readonly IMapper _mapper = mapper;

        public async Task<ApiResponse> Create(Category entity) => await _repository.Create(entity);

        public async Task<ApiResponse> CreateMany(IEnumerable<Category> entities)
        {
            var response = await _repository.CreateMany(entities);
            // Comprobar si hay elementos
            if (response.Result is null || response.Result is not IEnumerable<Category> category)
                return response;
            // Retornar Dtos
            response.Result = _mapper.Map<IEnumerable<CategoryDto>>(category);

            return response;
        }

        public async Task<ApiResponse> Delete(int id) => await _repository.Delete(id);

        public async Task<ApiResponse> GetAll()
        {
            var response = await _repository.GetAll();
            // Comprobar si hay elementos
            if (response.Result is null || response.Result is not IEnumerable<Category> category)
                return response;
            // Retornar Dtos
            response.Result = _mapper.Map<IEnumerable<CategoryDto>>(category);

            return response;
        }

        public async Task<ApiResponse> GetById(int id)
        {
            var response = await _repository.GetById(id);
            // Comprobar si hay un elemento
            if (response.Result is null || response.Result is not Category category)
                return response;
            // Retornar Dto
            response.Result = _mapper.Map<CategoryDto>(category);

            return response;
        }

        public async Task<ApiResponse> Update(Category entity) => await _repository.Update(entity);

        public async Task<ApiResponse> UpdateMany(IEnumerable<Category> entities) => await _repository.UpdateMany(entities);
    }
}
