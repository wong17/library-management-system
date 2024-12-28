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
        public async Task<ApiResponse> Create(AuthorInsertDto entity) => await repository.Create(mapper.Map<Author>(entity));

        public async Task<ApiResponse> CreateMany(IEnumerable<AuthorInsertDto> entities)
        {
            var response = await repository.CreateMany(mapper.Map<IEnumerable<Author>>(entities));
            // Comprobar si hay elementos
            if (response.Result is not IEnumerable<Author> authors)
                return response;
            // Retornar Dtos
            response.Result = mapper.Map<IEnumerable<AuthorDto>>(authors);

            return response;
        }

        public async Task<ApiResponse> Delete(int id) => await repository.Delete(id);

        public async Task<ApiResponse> GetAll()
        {
            var response = await repository.GetAll();
            // Comprobar si hay elementos
            if (response.Result is not IEnumerable<Author> authors)
                return response;
            // Retornar Dtos
            response.Result = mapper.Map<IEnumerable<AuthorDto>>(authors);

            return response;
        }

        public async Task<ApiResponse> GetById(int id)
        {
            var response = await repository.GetById(id);
            // Comprobar si hay un elemento
            if (response.Result is not Author author)
                return response;
            // Retornar Dto
            response.Result = mapper.Map<AuthorDto>(author);

            return response;
        }

        public async Task<ApiResponse> Update(AuthorUpdateDto entity) => await repository.Update(mapper.Map<Author>(entity));

        public async Task<ApiResponse> UpdateMany(IEnumerable<AuthorUpdateDto> entities) => 
            await repository.UpdateMany(mapper.Map<IEnumerable<Author>>(entities));
    }
}