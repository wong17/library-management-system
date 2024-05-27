using AutoMapper;
using LibraryManagementSystem.Bll.Interfaces.Library;
using LibraryManagementSystem.Common.Runtime;
using LibraryManagementSystem.Dal.Repository.Interfaces.Library;
using LibraryManagementSystem.Entities.Dtos.Library;
using LibraryManagementSystem.Entities.Models.Library;

namespace LibraryManagementSystem.Bll.Implements.Library
{
    public class AuthorBll(IAuthorRepository repository, IMapper mapper) : IAuthorBll
    {
        private readonly IAuthorRepository _repository = repository;
        private readonly IMapper _mapper = mapper;

        public async Task<ApiResponse> Create(Author entity) => await _repository.Create(entity);

        public async Task<ApiResponse> CreateMany(IEnumerable<Author> entities)
        {
            var response = await _repository.CreateMany(entities);
            // Comprobar si hay elementos
            if (response.Result is null || response.Result is not IEnumerable<Author> authors)
                return response;
            // Retornar Dtos
            response.Result = _mapper.Map<IEnumerable<AuthorDto>>(authors);

            return response;
        }

        public async Task<ApiResponse> Delete(int id) => await _repository.Delete(id);

        public async Task<ApiResponse> GetAll()
        {
            var response = await _repository.GetAll();
            // Comprobar si hay elementos
            if (response.Result is null || response.Result is not IEnumerable<Author> authors)
                return response;
            // Retornar Dtos
            response.Result = _mapper.Map<IEnumerable<AuthorDto>>(authors);

            return response;
        }

        public async Task<ApiResponse> GetById(int id)
        {
            var response = await _repository.GetById(id);
            // Comprobar si hay un elemento
            if (response.Result is null || response.Result is not Author author)
                return response;
            // Retornar Dto
            response.Result = _mapper.Map<AuthorDto>(author);

            return response;
        }

        public async Task<ApiResponse> Update(Author entity) => await _repository.Update(entity);

        public async Task<ApiResponse> UpdateMany(IEnumerable<Author> entities) => await _repository.UpdateMany(entities);
    }
}
